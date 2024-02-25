using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Farm;
using ProjectForFarmers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Geocoding;
using ProjectForFarmers.Application.Helpers;
using AutoMapper;
using Geocoding.Google;
using Address = ProjectForFarmers.Domain.Address;
using ProjectForFarmers.Application.Services.Auth;
using DayOfWeek = ProjectForFarmers.Domain.DayOfWeek;
using Microsoft.AspNetCore.Http;
using ProjectForFarmers.Application.ViewModels.Category;
using ProjectForFarmers.Application.ViewModels.Subcategory;

namespace ProjectForFarmers.Application.Services.Business
{
    public class FarmService : Service, IFarmService
    {
        private readonly string FarmsImageFolder;
        private readonly FileHelper FileHelper;
        private readonly EmailHelper EmailHelper;
        private readonly JwtService JwtService;

        public FarmService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            FarmsImageFolder = Configuration["File:Images:Farm"];
            FileHelper = new FileHelper();
            EmailHelper = new EmailHelper(configuration);
            JwtService = new JwtService(configuration);
        }

        public async Task Create(CreateFarmDto farmDto)
        {
            var farm = Mapper.Map<Farm>(farmDto);
            var address = farm.Address;

            var coords = await GetCoordinates(address);
            address.Latitude = coords.Latitude;
            address.Longitude = coords.Longitude;

            if(farmDto.Images != null)
            {
                farm.ImagesNames = await FileHelper.SaveImages(farmDto.Images, FarmsImageFolder);
            }
            else
            {
                farm.ImagesNames = new List<string>();
            }
            string token = await JwtService.EmailConfirmationToken(farm.Id, farmDto.ContactEmail);
            var owner = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == farm.OwnerId);

            string message = EmailContentBuilder.FarmEmailConfirmationMessageBody(farm.Name, owner.Name, owner.Surname, farmDto.ContactEmail, token);
            await EmailHelper.SendEmail(message, farmDto.ContactEmail, "Farm Email Confirmation");
            await DbContext.SaveChangesAsync();

            var farmLog = new FarmLog
            {
                Id = Guid.NewGuid(),
                PropertyName = null,
                Message = "FarmCreated",
                Parameters = null,
                CreationDate = DateTime.UtcNow,
                FarmId = farm.Id
            };

            DbContext.FarmsLogs.Add(farmLog);

            await DbContext.Farms.AddAsync(farm);
            await DbContext.SaveChangesAsync();
        }

        public void Validate(Guid? accountId, Guid id)
        {
            if (id != accountId)
            {
                string message = $"Access denied: Permission denied to modify data.";
                string userFacingMessage = CultureHelper.Exception("AccessDenied");

                throw new AuthorizationException(message, userFacingMessage);
            }
        }

        public async Task Delete(Guid farmId, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm == null)
            {
                string message = $"Farm with Id {farmId} was not found.";
                string userFacingMessage = CultureHelper.Exception("FarmWithIdNotFound", farmId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            Validate(ownerId, farm.OwnerId);

            var farmLog = new FarmLog
            {
                Id = Guid.NewGuid(),
                PropertyName = null,
                Message = "FarmDeleted",
                Parameters = null,
                CreationDate = DateTime.UtcNow,
                FarmId = farm.Id
            };

            DbContext.FarmsLogs.Add(farmLog);

            DbContext.Farms.Remove(farm);
            await FileHelper.DeleteFiles(farm.ImagesNames, FarmsImageFolder);
            await DbContext.SaveChangesAsync();
        }

        private async Task<Location> GetCoordinates(Address address)
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = Configuration["Geocoding:Apikey"] };
            var request = await geocoder.GeocodeAsync($"{address.Region} oblast, {address.District} district, {address.Settlement} street {address.Street}, {address.HouseNumber}, Ukraine");
            var coords = request.FirstOrDefault().Coordinates;
            return coords;
        }

        private async Task<Location> GetCoordinates(AddressDto dto)
        {
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = Configuration["Geocoding:Apikey"] };
            var request = await geocoder.GeocodeAsync($"{dto.Region} oblast, {dto.District} district, {dto.Settlement} street {dto.Street}, {dto.HouseNumber}, Ukraine");
            var coords = request.FirstOrDefault().Coordinates;
            return coords;
        }

        public async Task Update(UpdateFarmDto updateFarmDto, Guid ownerId)
        {
            var farm = await DbContext.Farms
                .Include(f => f.Owner)
                .Include(f => f.Address)
                .Include(f => f.PaymentData)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Monday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Tuesday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Wednesday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Thursday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Friday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Saturday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Sunday)
                .FirstOrDefaultAsync(f => f.Id == updateFarmDto.Id);

            if (farm == null)
            {
                string message = $"Farm with Id {updateFarmDto.Id} was not found.";
                string userFacingMessage = CultureHelper.Exception("FarmWithIdNotFound", updateFarmDto.Id.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            Validate(ownerId, farm.OwnerId);

            LogAndUpdateIfChanged("Name", farm.Name, updateFarmDto.Name, () => farm.Name = updateFarmDto.Name, farm.Id);
            LogAndUpdateIfChanged("Description", farm.Description, updateFarmDto.Description, () => farm.Description = updateFarmDto.Description, farm.Id);
            LogAndUpdateIfChanged("ContactEmail", farm.ContactEmail, updateFarmDto.ContactEmail, async () => 
            {
                string token = await JwtService.EmailConfirmationToken(farm.Id, updateFarmDto.ContactEmail);
                string message = EmailContentBuilder.FarmEmailConfirmationMessageBody(farm.Name, farm.Owner.Name, farm.Owner.Surname, updateFarmDto.ContactEmail, token);
                await EmailHelper.SendEmail(message, updateFarmDto.ContactEmail, "Farm Email Confirmation");
            }, farm.Id);
            LogAndUpdateIfChanged("ContactPhone", farm.ContactPhone, updateFarmDto.ContactPhone, () => farm.ContactPhone = updateFarmDto.ContactPhone, farm.Id);
            LogAndUpdateIfChanged("SocialPageUrl", farm.FirstSocialPageUrl, updateFarmDto.FirstSocialPageUrl, () => farm.FirstSocialPageUrl = updateFarmDto.FirstSocialPageUrl, farm.Id);
            LogAndUpdateIfChanged("SocialPageUrl", farm.SecondSocialPageUrl, updateFarmDto.SecondSocialPageUrl, () => farm.SecondSocialPageUrl = updateFarmDto.SecondSocialPageUrl, farm.Id);
            UpdateReceivingTypes(farm, updateFarmDto);
            
            await DbContext.SaveChangesAsync();

            await UpdateAddress(farm, updateFarmDto.Address);
            await UpdateSchedule(farm, updateFarmDto.Schedule);
            await UpdateImages(farm, updateFarmDto.Images);

            await DbContext.SaveChangesAsync();
        }

        private void UpdateReceivingTypes(Farm farm, UpdateFarmDto updateFarmDto)
        {
            farm.ReceivingMethods = farm.ReceivingMethods.OrderBy(x => x).ToList();
            updateFarmDto.ReceivingMethods = updateFarmDto.ReceivingMethods.OrderBy(x => x).ToList();

            if (!farm.ReceivingMethods.SequenceEqual(updateFarmDto.ReceivingMethods))
            {
                var farmLog = new FarmLog
                {
                    Id = Guid.NewGuid(),
                    PropertyName = "ReceivingMethods",
                    Message = "PropertyChanged",
                    Parameters = new string[]
                    {
                        string.Join(", ", farm.ReceivingMethods),
                        string.Join(", ", updateFarmDto.ReceivingMethods)
                    },
                    CreationDate = DateTime.UtcNow,
                    FarmId = farm.Id
                };

                DbContext.FarmsLogs.Add(farmLog);

                farm.ReceivingMethods = updateFarmDto.ReceivingMethods;
            }

            
        }

        private void LogAndUpdateIfChanged(string propertyName, string oldValue, string newValue, Action updateAction, Guid farmId)
        {
            if (oldValue != newValue)
            {
                updateAction.Invoke();
                var farmLog = new FarmLog
                {
                    Id = Guid.NewGuid(),
                    PropertyName = propertyName,
                    Message = "PropertyChanged",
                    Parameters = new string[] { oldValue, newValue },
                    CreationDate = DateTime.UtcNow,
                    FarmId = farmId
                };
                
                DbContext.FarmsLogs.Add(farmLog);
            }
        }

        private async Task UpdateImages(Farm farm, List<IFormFile> images)
        {
            if (farm.ImagesNames == null) 
                farm.ImagesNames = new List<string>();

            List<string> deletedImages = new List<string>();
            List<string> newImages = new List<string>();

            if (images != null)
            {
                foreach (var imageName in farm.ImagesNames)
                {
                    if (!images.Any(file => file.FileName == imageName))
                    {
                        FileHelper.DeleteFile(imageName, FarmsImageFolder);
                        deletedImages.Add(imageName);
                    }
                }
            }

            if (images != null)
            {
                foreach (var newImage in images)
                {
                    if (!farm.ImagesNames.Contains(newImage.FileName))
                    {
                        string imageName = await FileHelper.SaveFile(newImage, FarmsImageFolder);
                        farm.ImagesNames.Add(imageName);
                        newImages.Add(imageName);
                    }
                }
            }

            if(deletedImages.Count > 0)
            {
                foreach(var deletedImage in deletedImages)
                {
                    farm.ImagesNames.Remove(deletedImage);
                }

                var deletedImagesLog = new FarmLog
                {
                    Id = Guid.NewGuid(),
                    PropertyName = null,
                    Message = "ImagesDeleted",
                    Parameters = new string[] { string.Join(", ", deletedImages)},
                    CreationDate = DateTime.UtcNow,
                    FarmId = farm.Id
                };

                DbContext.FarmsLogs.Add(deletedImagesLog);

                var newImagesLog = new FarmLog
                {
                    Id = Guid.NewGuid(),
                    PropertyName = null,
                    Message = "ImagesCreated",
                    Parameters = new string[] { string.Join(", ", newImages) },
                    CreationDate = DateTime.UtcNow,
                    FarmId = farm.Id
                };

                DbContext.FarmsLogs.Add(newImagesLog);
            }
        }

        private async Task UpdateSchedule(Farm farm, ScheduleDto scheduleDto)
        {
            var schedule = farm.Schedule;

            await UpdateDayOfWeek("Monday", schedule.Monday, scheduleDto.Monday, farm.Id);
            await UpdateDayOfWeek("Tuesday", schedule.Tuesday, scheduleDto.Tuesday, farm.Id);
            await UpdateDayOfWeek("Wednesday", schedule.Wednesday, scheduleDto.Wednesday, farm.Id);
            await UpdateDayOfWeek("Thursday", schedule.Thursday, scheduleDto.Thursday, farm.Id);
            await UpdateDayOfWeek("Friday", schedule.Friday, scheduleDto.Friday, farm.Id);
            await UpdateDayOfWeek("Saturday", schedule.Saturday, scheduleDto.Saturday, farm.Id);
            await UpdateDayOfWeek("Sunday", schedule.Sunday, scheduleDto.Sunday, farm.Id);

            await DbContext.SaveChangesAsync();
        }

        private async Task UpdateDayOfWeek(string dayName, DayOfWeek dayOfWeek, DayOfWeekDto dayOfWeekDto, Guid farmId)
        {
            if (dayOfWeek.IsOpened == dayOfWeekDto.IsOpened &&
                dayOfWeek.StartHour == dayOfWeekDto.StartHour &&
                dayOfWeek.StartMinute == dayOfWeekDto.StartMinute &&
                dayOfWeek.EndHour == dayOfWeekDto.EndHour &&
                dayOfWeek.EndMinute == dayOfWeekDto.EndMinute)
            {
                return;
            }

            dayOfWeek.IsOpened = dayOfWeekDto.IsOpened;
            dayOfWeek.StartHour = dayOfWeekDto.StartHour;
            dayOfWeek.StartMinute = dayOfWeekDto.StartMinute;
            dayOfWeek.EndHour = dayOfWeekDto.EndHour;
            dayOfWeek.EndMinute = dayOfWeekDto.EndMinute;

            var farmLog = new FarmLog
            {
                Id = Guid.NewGuid(),
                PropertyName = dayName,
                Message = "DayScheduleChanged",
                Parameters = new string[] 
                {
                    dayOfWeekDto.IsOpened.ToString(),
                    dayOfWeekDto.StartHour.ToString(),
                    dayOfWeekDto.StartMinute.ToString(),
                    dayOfWeekDto.EndHour.ToString(),
                    dayOfWeekDto.EndMinute.ToString()
                },
                CreationDate = DateTime.UtcNow,
                FarmId = farmId
            };

            DbContext.FarmsLogs.Add(farmLog);
        }

        private async Task UpdateAddress(Farm farm, AddressDto addressDto)
        {
            var address = farm.Address;

            if (address.Region != addressDto.Region
                || address.District != addressDto.District
                || address.Settlement != addressDto.Settlement
                || address.Street != addressDto.Street
                || address.HouseNumber != addressDto.HouseNumber)
            {
                var coords = await GetCoordinates(addressDto);

                LogAndUpdateIfChanged("Latitude", address.Latitude.ToString(), coords.Latitude.ToString(), () => address.Latitude = coords.Latitude, farm.Id);
                LogAndUpdateIfChanged("Longitude", address.Longitude.ToString(), coords.Longitude.ToString(), () => address.Longitude = coords.Longitude, farm.Id);
            }

            LogAndUpdateIfChanged("Region", address.Region, addressDto.Region, () => address.Region = addressDto.Region, farm.Id);
            LogAndUpdateIfChanged("District", address.District, addressDto.District, () => address.District = addressDto.District, farm.Id);
            LogAndUpdateIfChanged("Settlement", address.Settlement, addressDto.Settlement, () => address.Settlement = addressDto.Settlement, farm.Id);
            LogAndUpdateIfChanged("Street", address.Street, addressDto.Street, () => address.Street = addressDto.Street, farm.Id);
            LogAndUpdateIfChanged("HouseNumber", address.HouseNumber, addressDto.HouseNumber, () => address.HouseNumber = addressDto.HouseNumber, farm.Id);
            LogAndUpdateIfChanged("PostalCode", address.PostalCode, addressDto.PostalCode, () => address.PostalCode = addressDto.PostalCode, farm.Id);
            LogAndUpdateIfChanged("Note", address.Note, addressDto.Note, () => address.Note = addressDto.Note, farm.Id);

            await DbContext.SaveChangesAsync();
        }

        public async Task<FarmVm> Get(Guid farmId)
        {
            var farm = await DbContext.Farms
                .Include(f => f.Owner)
                .Include(f => f.Address)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Monday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Tuesday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Wednesday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Thursday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Friday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Sunday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Saturday)
                .Include(f => f.PaymentData)
                .Include(f => f.Logs)
                .FirstOrDefaultAsync(f => f.Id == farmId);

            if (farm == null) 
            {
                string message = $"Farm with Id {farmId} was not found.";
                string userFacingMessage = CultureHelper.Exception("FarmWithIdNotFound", farmId.ToString());
                throw new NotFoundException(message, userFacingMessage);
            } 

            var vm = Mapper.Map<FarmVm>(farm);

            foreach(var log in farm.Logs)
            {
                var logVm = new FarmLogVm(GetMessage(log), log.CreationDate);
                vm.Logs.Add(logVm);
            }

            foreach(var categoryId in farm.Categories) 
            {
                var category = DbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category == null)
                {
                    string message = $"Category with Id {categoryId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", categoryId.ToString());

                    throw new NotFoundException(message, userFacingMessage);
                }

                vm.Categories.Add(new CategoryLookupVm(category.Id, category.Name));
            }

            foreach (var subcategoryId in farm.Subcategories)
            {
                var subcategory = DbContext.Subcategories.FirstOrDefault(c => c.Id == subcategoryId);
                if (subcategory == null)
                {
                    string message = $"Subcategory with Id {subcategoryId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("SubcategoryWithIdNotFound", subcategoryId.ToString());
                    throw new NotFoundException(message, userFacingMessage);
                }

                vm.Subcategories.Add(new SubcategoryVm(subcategory.Id, subcategory.Name, subcategory.CategoryId));
            }

            return vm;
        }

        private string GetMessage(FarmLog log)
        {
            string message = "";

            if (log == null || log.Message == null)
            {
                return "";
            }
            else if (log.PropertyName != null && log.PropertyName != "")
            {
                message += (CultureHelper.Property(log.PropertyName) + " ");
            }

            message += CultureHelper.FarmLog(log.Message);
            if (log.Parameters != null && log.Parameters.Length > 0)
            {
                message = string.Format(message, log.Parameters);
            }

            return message;
        }

        public async Task<FarmListVm> GetAll(Guid userId)
        {
            var farms = DbContext.Farms.Where(f => f.OwnerId == userId).ToArray();
            var response = new FarmListVm();
            foreach ( var f in farms )
            {
                var vm = Mapper.Map<FarmLookupVm>(f);
                response.Farms.Add(vm);
            }

            return response;
        }

        public async Task UpdateFarmCategoriesAndSubcategories(UpdateFarmCategoriesAndSubcategoriesDto updateFarmCategoriesAndSubcategoriesDto, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmCategoriesAndSubcategoriesDto.FarmId);
            if (farm == null)
            {
                string message = $"Farm with Id {updateFarmCategoriesAndSubcategoriesDto.FarmId} was not found.";
                string userFacingMessage = CultureHelper.Exception("FarmWithIdNotFound", updateFarmCategoriesAndSubcategoriesDto.FarmId.ToString());
                throw new NotFoundException(message, userFacingMessage);
            }

            Validate(ownerId, farm.OwnerId);

            farm.Categories = updateFarmCategoriesAndSubcategoriesDto.Categories;
            farm.Subcategories = updateFarmCategoriesAndSubcategoriesDto.Subcategories;

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateSettings(UpdateFarmSettingsDto updateFarmSettingsDto, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmSettingsDto.FarmId);
            if (farm == null)
            {
                string message = $"Farm with Id {updateFarmSettingsDto.FarmId} was not found.";
                string userFacingMessage = CultureHelper.Exception("FarmWithIdNotFound", updateFarmSettingsDto.FarmId.ToString());
                throw new NotFoundException(message, userFacingMessage);
            }

            Validate(ownerId, farm.OwnerId);

            if (farm.PaymentData == null)
                farm.PaymentData = new PaymentData();

            if(updateFarmSettingsDto.PaymentData != null)
            {
                LogAndUpdateIfChanged("CardNumber", farm.PaymentData.CardNumber, updateFarmSettingsDto.PaymentData.CardNumber, () => farm.PaymentData.CardNumber = updateFarmSettingsDto.PaymentData.CardNumber, farm.Id);
                LogAndUpdateIfChanged("AccountNumber", farm.PaymentData.AccountNumber, updateFarmSettingsDto.PaymentData.AccountNumber, () => farm.PaymentData.AccountNumber = updateFarmSettingsDto.PaymentData.AccountNumber, farm.Id);
                LogAndUpdateIfChanged("BankUSREOU", farm.PaymentData.BankUSREOU, updateFarmSettingsDto.PaymentData.BankUSREOU, () => farm.PaymentData.BankUSREOU = updateFarmSettingsDto.PaymentData.BankUSREOU, farm.Id);
                LogAndUpdateIfChanged("BIC", farm.PaymentData.BIC, updateFarmSettingsDto.PaymentData.BIC, () => farm.PaymentData.BIC = updateFarmSettingsDto.PaymentData.BIC, farm.Id);
                LogAndUpdateIfChanged("HolderFullName", farm.PaymentData.HolderFullName, updateFarmSettingsDto.PaymentData.HolderFullName, () => farm.PaymentData.HolderFullName = updateFarmSettingsDto.PaymentData.HolderFullName, farm.Id);
            }

            if(farm.ReceivingMethods == null) 
                farm.PaymentTypes = new List<PaymentType>() { PaymentType.Cash };  

            if (updateFarmSettingsDto.HasOnlinePayment 
                && !farm.PaymentTypes.Contains(PaymentType.Online))
            {
                farm.PaymentTypes.Add(PaymentType.Online);
                var farmLog = new FarmLog
                {
                    Id = Guid.NewGuid(),
                    PropertyName = null,
                    Message = "AddedOnlinePayment",
                    Parameters = null,
                    CreationDate = DateTime.UtcNow,
                    FarmId = farm.Id
                };
            }
            else if (!updateFarmSettingsDto.HasOnlinePayment
                && farm.PaymentTypes.Contains(PaymentType.Online))
            {
                farm.PaymentTypes.Remove(PaymentType.Online);
                var farmLog = new FarmLog
                {
                    Id = Guid.NewGuid(),
                    PropertyName = null,
                    Message = "DeletedOnlinePayment",
                    Parameters = null,
                    CreationDate = DateTime.UtcNow,
                    FarmId = farm.Id
                };
            }

            await DbContext.SaveChangesAsync();
        }
    }
}
