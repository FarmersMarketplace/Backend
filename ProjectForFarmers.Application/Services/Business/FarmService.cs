﻿using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.Services.Auth;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Farm;
using FarmersMarketplace.Application.ViewModels.Producers;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Payment;
using Geocoding;
using Geocoding.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Application.Services.Business
{
    public class FarmService : Service, IFarmService
    {
        private readonly string FarmsImageFolder;
        private readonly FileHelper FileHelper;
        private readonly EmailHelper EmailHelper;
        private readonly JwtService JwtService;
        private readonly CoordinateHelper CoordinateHelper;
        private readonly ICacheProvider<Farm> CacheProvider;
        private readonly ISearchSynchronizer<Farm> SearchSynchronizer;
        private readonly FarmTrendService TrendService;

        public FarmService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration, ISearchSynchronizer<Farm> farmSynchronizer, ICacheProvider<Farm> cacheProvider) : base(mapper, dbContext, configuration)
        {
            FarmsImageFolder = Configuration["File:Images:Farm"];
            FileHelper = new FileHelper();
            EmailHelper = new EmailHelper(configuration);
            JwtService = new JwtService(configuration);
            CoordinateHelper = new CoordinateHelper(configuration);
            SearchSynchronizer = farmSynchronizer;
            CacheProvider = cacheProvider;
            TrendService = new FarmTrendService(dbContext, mapper, 5, 5);
        }

        public async Task Create(CreateFarmDto dto)
        {
            var farm = Mapper.Map<Farm>(dto);
            var address = farm.Address;

            var coords = await CoordinateHelper.GetCoordinates(address);
            address.Latitude = coords.Latitude;
            address.Longitude = coords.Longitude;

            if(dto.Images != null)
            {
                farm.ImagesNames = await FileHelper.SaveImages(dto.Images, FarmsImageFolder);
            }
            else
            {
                farm.ImagesNames = new List<string>();
            }

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
            await SearchSynchronizer.Create(farm);
        }

        public void Validate(Guid? accountId, Guid id)
        {
            if (id != accountId)
            {
                string message = $"Access denied: Permission denied to modify data.";
                throw new AuthorizationException(message, "AccessDenied");
            }
        }

        public async Task Delete(Guid farmId, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm == null)
            {
                string message = $"Farm with Id {farmId} was not found.";
                throw new NotFoundException(message, "FarmNotFound");
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
            await SearchSynchronizer.Delete(farm.Id);
            await CacheProvider.Delete(farm.Id);
            await TrendService.UpdateIfPopular(farm);
        }

        private async Task<Geocoding.Location> GetCoordinates(AddressDto dto)
        {
            var key = Configuration["Geocoding:Apikey"];
            IGeocoder geocoder = new GoogleGeocoder() { ApiKey = key };
            var request = await geocoder.GeocodeAsync($"{dto.Region} oblast, {dto.District} district, {dto.Settlement} street {dto.Street}, {dto.HouseNumber}, Ukraine");
            var coords = request.FirstOrDefault().Coordinates;

            return coords;
        }

        public async Task Update(UpdateFarmDto dto, Guid ownerId)
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
                .FirstOrDefaultAsync(f => f.Id == dto.Id);

            if (farm == null)
            {
                string message = $"Farm with Id {dto.Id} was not found.";
                throw new NotFoundException(message, "FarmNotFound");
            }

            Validate(ownerId, farm.OwnerId);

            LogAndUpdateIfChanged("Name", farm.Name, dto.Name, () => farm.Name = dto.Name, farm.Id);
            LogAndUpdateIfChanged("Description", farm.Description, dto.Description, () => farm.Description = dto.Description, farm.Id);
            LogAndUpdateIfChanged("ContactPhone", farm.Phone, dto.ContactPhone, () => farm.Phone = dto.ContactPhone, farm.Id);
            LogAndUpdateIfChanged("SocialPageUrl", farm.FirstSocialPageUrl, dto.FirstSocialPageUrl, () => farm.FirstSocialPageUrl = dto.FirstSocialPageUrl, farm.Id);
            LogAndUpdateIfChanged("SocialPageUrl", farm.SecondSocialPageUrl, dto.SecondSocialPageUrl, () => farm.SecondSocialPageUrl = dto.SecondSocialPageUrl, farm.Id);
            UpdateReceivingTypes(farm, dto);
            
            await DbContext.SaveChangesAsync();

            await UpdateAddress(farm, dto.Address);
            await UpdateSchedule(farm, dto.Schedule);
            await UpdateImages(farm, dto.Images);

            await DbContext.SaveChangesAsync();
            await SearchSynchronizer.Update(farm);
            await CacheProvider.Update(farm);
            await TrendService.UpdateIfPopular(farm);
        }

        private void UpdateReceivingTypes(Farm farm, UpdateFarmDto dto)
        {
            farm.ReceivingMethods = farm.ReceivingMethods.OrderBy(x => x).ToList();
            dto.ReceivingMethods = dto.ReceivingMethods.OrderBy(x => x).ToList();

            if (!farm.ReceivingMethods.SequenceEqual(dto.ReceivingMethods))
            {
                var farmLog = new FarmLog
                {
                    Id = Guid.NewGuid(),
                    PropertyName = "ReceivingMethods",
                    Message = "PropertyChanged",
                    Parameters = new string[]
                    {
                        string.Join(", ", farm.ReceivingMethods),
                        string.Join(", ", dto.ReceivingMethods)
                    },
                    CreationDate = DateTime.UtcNow,
                    FarmId = farm.Id
                };

                DbContext.FarmsLogs.Add(farmLog);

                farm.ReceivingMethods = dto.ReceivingMethods;
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

        private async Task UpdateSchedule(Farm farm, ScheduleDto dto)
        {
            var schedule = farm.Schedule;

            await UpdateDayOfWeek("Monday", schedule.Monday, dto.Monday, farm.Id);
            await UpdateDayOfWeek("Tuesday", schedule.Tuesday, dto.Tuesday, farm.Id);
            await UpdateDayOfWeek("Wednesday", schedule.Wednesday, dto.Wednesday, farm.Id);
            await UpdateDayOfWeek("Thursday", schedule.Thursday, dto.Thursday, farm.Id);
            await UpdateDayOfWeek("Friday", schedule.Friday, dto.Friday, farm.Id);
            await UpdateDayOfWeek("Saturday", schedule.Saturday, dto.Saturday, farm.Id);
            await UpdateDayOfWeek("Sunday", schedule.Sunday, dto.Sunday, farm.Id);

            await DbContext.SaveChangesAsync();
        }

        private async Task UpdateDayOfWeek(string dayName, DayOfWeek dayOfWeek, DayOfWeekDto dto, Guid farmId)
        {
            if (dayOfWeek.IsOpened == dto.IsOpened &&
                dayOfWeek.StartHour == dto.StartHour &&
                dayOfWeek.StartMinute == dto.StartMinute &&
                dayOfWeek.EndHour == dto.EndHour &&
                dayOfWeek.EndMinute == dto.EndMinute)
            {
                return;
            }

            dayOfWeek.IsOpened = dto.IsOpened;
            dayOfWeek.StartHour = dto.StartHour;
            dayOfWeek.StartMinute = dto.StartMinute;
            dayOfWeek.EndHour = dto.EndHour;
            dayOfWeek.EndMinute = dto.EndMinute;

            var farmLog = new FarmLog
            {
                Id = Guid.NewGuid(),
                PropertyName = dayName,
                Message = "DayScheduleChanged",
                Parameters = new string[] 
                {
                    dto.IsOpened.ToString(),
                    dto.StartHour.ToString(),
                    dto.StartMinute.ToString(),
                    dto.EndHour.ToString(),
                    dto.EndMinute.ToString()
                },
                CreationDate = DateTime.UtcNow,
                FarmId = farmId
            };

            DbContext.FarmsLogs.Add(farmLog);
        }

        private async Task UpdateAddress(Farm farm, AddressDto dto)
        {
            var address = farm.Address;

            if (address.Region != dto.Region
                || address.District != dto.District
                || address.Settlement != dto.Settlement
                || address.Street != dto.Street
                || address.HouseNumber != dto.HouseNumber)
            {
                var coords = await GetCoordinates(dto);

                LogAndUpdateIfChanged("Latitude", address.Latitude.ToString(), coords.Latitude.ToString(), () => address.Latitude = coords.Latitude, farm.Id);
                LogAndUpdateIfChanged("Longitude", address.Longitude.ToString(), coords.Longitude.ToString(), () => address.Longitude = coords.Longitude, farm.Id);
            }

            LogAndUpdateIfChanged("Region", address.Region, dto.Region, () => address.Region = dto.Region, farm.Id);
            LogAndUpdateIfChanged("District", address.District, dto.District, () => address.District = dto.District, farm.Id);
            LogAndUpdateIfChanged("Settlement", address.Settlement, dto.Settlement, () => address.Settlement = dto.Settlement, farm.Id);
            LogAndUpdateIfChanged("Street", address.Street, dto.Street, () => address.Street = dto.Street, farm.Id);
            LogAndUpdateIfChanged("HouseNumber", address.HouseNumber, dto.HouseNumber, () => address.HouseNumber = dto.HouseNumber, farm.Id);
            LogAndUpdateIfChanged("PostalCode", address.PostalCode, dto.PostalCode, () => address.PostalCode = dto.PostalCode, farm.Id);
            LogAndUpdateIfChanged("Note", address.Note, dto.Note, () => address.Note = dto.Note, farm.Id);

            await DbContext.SaveChangesAsync();
        }

        public async Task<FarmForProducerVm> GetForProducer(Guid farmId)
        {
            if (CacheProvider.Exists(farmId))
            {
                var f = await CacheProvider.Get(farmId);
                return Mapper.Map<FarmForProducerVm>(f);
            }

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
                throw new NotFoundException(message, "FarmNotFound");
            } 

            var vm = Mapper.Map<FarmForProducerVm>(farm);

            if (farm.PaymentTypes != null
                && farm.PaymentTypes.Contains(PaymentType.Online))
            {
                vm.PaymentData.HasOnlinePayment = true;
            }
            else
            {
                vm.PaymentData.HasOnlinePayment = false;
            }

            foreach (var log in farm.Logs)
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
                    throw new NotFoundException(message, "CategoryNotFound");
                }

                vm.Categories.Add(new CategoryLookupVm(category.Id, category.Name));
            }

            foreach (var subcategoryId in farm.Subcategories)
            {
                var subcategory = DbContext.Subcategories.FirstOrDefault(c => c.Id == subcategoryId);
                if (subcategory == null)
                {
                    string message = $"Subcategory with Id {subcategoryId} was not found.";
                    throw new NotFoundException(message, "SubcategoryNotFound");
                }

                vm.Subcategories.Add(new SubcategoryVm(subcategory.Id, subcategory.Name, subcategory.CategoryId));
            }

            await CacheProvider.Set(farm);

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

        public async Task<FarmListVm> GetAllForProducer(Guid userId)
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

        public async Task UpdateFarmCategoriesAndSubcategories(UpdateFarmCategoriesAndSubcategoriesDto dto, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == dto.FarmId);
            if (farm == null)
            {
                string message = $"Farm with Id {dto.FarmId} was not found.";
                throw new NotFoundException(message, "FarmNotFound");
            }

            Validate(ownerId, farm.OwnerId);

            farm.Categories = dto.Categories;
            farm.Subcategories = dto.Subcategories;

            await DbContext.SaveChangesAsync();
            await SearchSynchronizer.Update(farm);
            await CacheProvider.Update(farm);
        }

        public async Task UpdatePaymentData(FarmPaymentDataDto dto, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == dto.FarmId);
            if (farm == null)
            {
                string message = $"Farm with Id {dto.FarmId} was not found.";
                throw new NotFoundException(message, "FarmFound");
            }

            Validate(ownerId, farm.OwnerId);

            if (farm.PaymentData == null)
                farm.PaymentData = new ProducerPaymentData();

            if(dto.MainPaymentData == null)
                dto.MainPaymentData = MainPaymentData.Card;

            LogAndUpdateIfChanged("CardNumber", farm.PaymentData.CardNumber, dto.CardNumber, () => farm.PaymentData.CardNumber = dto.CardNumber, farm.Id);
            LogAndUpdateIfChanged("AccountNumber", farm.PaymentData.AccountNumber, dto.AccountNumber, () => farm.PaymentData.AccountNumber = dto.AccountNumber, farm.Id);
            LogAndUpdateIfChanged("BankUSREOU", farm.PaymentData.BankUSREOU, dto.BankUSREOU, () => farm.PaymentData.BankUSREOU = dto.BankUSREOU, farm.Id);
            LogAndUpdateIfChanged("BIC", farm.PaymentData.BIC, dto.BIC, () => farm.PaymentData.BIC = dto.BIC, farm.Id);
            LogAndUpdateIfChanged("CardExpirationYear", farm.PaymentData.CardExpirationYear, dto.CardExpirationYear, () => farm.PaymentData.CardExpirationYear = dto.CardExpirationYear, farm.Id);
            LogAndUpdateIfChanged("CardExpirationMonth", farm.PaymentData.CardExpirationMonth, dto.CardExpirationMonth, () => farm.PaymentData.CardExpirationMonth = dto.CardExpirationMonth, farm.Id);
            LogAndUpdateIfChanged("MainPaymentData", farm.PaymentData.MainPaymentData.ToString(), dto.MainPaymentData.ToString(), () => farm.PaymentData.MainPaymentData = (MainPaymentData)dto.MainPaymentData, farm.Id);

            if (farm.ReceivingMethods == null) 
                farm.PaymentTypes = new List<PaymentType>() { PaymentType.Cash };  

            if (dto.HasOnlinePayment 
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
            else if (!dto.HasOnlinePayment
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
            await SearchSynchronizer.Update(farm);
            await CacheProvider.Update(farm);
        }

        public async Task<CardDataVm> CopyOwnerCardData(Guid ownerId)
        {
            var farmer = await DbContext.Farmers.Include(c => c.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == ownerId);

            if (farmer == null)
            {
                string message = $"Account with Id {ownerId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            var vm = new CardDataVm
            {
                CardNumber = farmer.PaymentData.CardNumber,
                CardExpirationMonth = farmer.PaymentData.CardExpirationMonth,
                CardExpirationYear = farmer.PaymentData.CardExpirationYear,
            };

            return vm;
        }

        public async Task<AccountNumberDataVm> CopyOwnerAccountNumberData(Guid ownerId)
        {
            var farmer = await DbContext.Farmers.Include(c => c.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == ownerId);

            if (farmer == null)
            {
                string message = $"Account with Id {ownerId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            var vm = new AccountNumberDataVm
            {
                AccountNumber = farmer.PaymentData.AccountNumber,
                BankUSREOU = farmer.PaymentData.BankUSREOU,
                BIC = farmer.PaymentData.BIC,
            };

            if (farmer.PaymentTypes != null
                && farmer.PaymentTypes.Contains(PaymentType.Online))
            {
                vm.HasOnlinePayment = true;
            }
            else
            {
                vm.HasOnlinePayment = false;
            }

            return vm;
        }

        public async Task<FarmForCustomerVm> GetForCustomer(Guid farmId)
        {
            if (CacheProvider.Exists(farmId))
            {
                var f = await CacheProvider.Get(farmId);
                return Mapper.Map<FarmForCustomerVm>(f);
            }

            var farm = await DbContext.Farms
                .Include(f => f.Feedbacks)
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
                throw new NotFoundException(message, "FarmNotFound");
            }

            await CacheProvider.Set(farm);
            var vm = Mapper.Map<FarmForCustomerVm>(farm);

            return vm;
        }

        public async Task<ProducerListVm> GetPopular()
        {
            var vm = await TrendService.UpdateAndGet();
            return vm;
        }
    }
}
