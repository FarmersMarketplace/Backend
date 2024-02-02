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

namespace ProjectForFarmers.Application.Services.Business
{
    public class FarmService : Service, IFarmService
    {
        private readonly string FarmsImageFolder;
        private readonly ImageHelper ImageHelper;
        private readonly EmailHelper EmailHelper;
        private readonly JwtService JwtService;

        public FarmService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            FarmsImageFolder = Configuration["Images:Farms"];
            ImageHelper = new ImageHelper();
            EmailHelper = new EmailHelper(configuration);
            JwtService = new JwtService(configuration);
        }

        public async Task Create(CreateFarmDto createFarmDto)
        {
            var farm = Mapper.Map<Farm>(createFarmDto);
            var address = farm.Address;

            var coords = await GetCoordinates(address);
            address.Latitude = coords.Latitude;
            address.Longtitude = coords.Longitude;

            if(createFarmDto.Images != null)
            {
                farm.ImagesNames = await ImageHelper.SaveImages(createFarmDto.Images, FarmsImageFolder);
            }
            else
            {
                farm.ImagesNames = new List<string>();
            }
            string token = await JwtService.EmailConfirmationToken(farm.Id, createFarmDto.ContactEmail);
            var owner = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == farm.OwnerId);

            string message = EmailContentBuilder.FarmEmailConfirmationMessageBody(farm.Name, owner.Name, owner.Surname, createFarmDto.ContactEmail, token);
            await EmailHelper.SendEmail(message, createFarmDto.ContactEmail, "Farm Email Confirmation");

            await DbContext.Farms.AddAsync(farm);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid farmId, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == farmId && f.OwnerId == ownerId);
            if (farm == null) throw new NotFoundException($"Farm with Id {farmId} does not exist.");
            DbContext.Farms.Remove(farm);
            await ImageHelper.DeleteImages(farm.ImagesNames, FarmsImageFolder);
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

        public async Task Update(UpdateFarmDto updateFarmDto)
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
                    .ThenInclude(s => s.Saturday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Sunday)
                .FirstOrDefaultAsync(f => f.Id == updateFarmDto.FarmId);

            if (farm == null) throw new NotFoundException($"Farm with Id {updateFarmDto.FarmId} does not exist.");

            await UpdateAddress(farm.Address, updateFarmDto.Address);
            await UpdateSchedule(farm.Schedule, updateFarmDto.Schedule);
            await UpdateFarm(farm, updateFarmDto);

            await DbContext.SaveChangesAsync();
        }

        private async Task UpdateFarm(Farm farm, UpdateFarmDto updateFarmDto)
        {
            farm.Name = updateFarmDto.Name;
            farm.Description = updateFarmDto.Description;
            farm.ContactEmail = updateFarmDto.ContactEmail;
            farm.ContactPhone = updateFarmDto.ContactPhone;
            farm.SocialPageUrl = updateFarmDto.SocialPageUrl;
        }

        private async Task UpdateSchedule(Schedule schedule, ScheduleDto scheduleDto)
        {
            await UpdateDayOfWeek(schedule.Monday, scheduleDto.Monday);
            await UpdateDayOfWeek(schedule.Tuesday, scheduleDto.Tuesday);
            await UpdateDayOfWeek(schedule.Wednesday, scheduleDto.Wednesday);
            await UpdateDayOfWeek(schedule.Thursday, scheduleDto.Thursday);
            await UpdateDayOfWeek(schedule.Friday, scheduleDto.Friday);
            await UpdateDayOfWeek(schedule.Saturday, scheduleDto.Saturday);
            await UpdateDayOfWeek(schedule.Sunday, scheduleDto.Sunday);
        }

        private async Task UpdateDayOfWeek(DayOfWeek dayOfWeek, DayOfWeekDto dayOfWeekDto)
        {
            dayOfWeek.IsOpened = dayOfWeekDto.IsOpened;
            dayOfWeek.StartHour = dayOfWeekDto.StartHour;
            dayOfWeek.StartMinute = dayOfWeekDto.StartMinute;
            dayOfWeek.EndHour = dayOfWeekDto.EndHour;
            dayOfWeek.EndMinute = dayOfWeekDto.EndMinute;
        }

        private async Task UpdateAddress(Address address, AddressDto addressDto)
        {
            if (address.Region != addressDto.Region
                || address.District != addressDto.District
                || address.Settlement != addressDto.Settlement
                || address.Street != addressDto.Street
                || address.HouseNumber != addressDto.HouseNumber)
            {
                var coords = await GetCoordinates(addressDto);
                address.Latitude = coords.Latitude;
                address.Longtitude = coords.Longitude;
            }

            address.Region = addressDto.Region;
            address.District = addressDto.District;
            address.Settlement = addressDto.Settlement;
            address.Street = addressDto.Street;
            address.HouseNumber = addressDto.HouseNumber;
            address.PostalCode = addressDto.PostalCode;
            address.Note = addressDto.Note;
        }

        public async Task<FarmVm> Get(Guid farmId)
        {
            var farm = await DbContext.Farms
                .Include(f => f.Owner)
                .Include(f => f.Address)
                .Include(f => f.Schedule)
                .FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {farmId} does not exist.");

            var request = new FarmVm
            {
                Id = farm.Id,
                Name = farm.Name,
                Description = farm.Description,
                ContactEmail = farm.ContactEmail,
                OwnerName = farm.Owner.Name + " " + farm.Owner.Surname,
                Address = farm.Address,
                SocialPageUrl = farm.SocialPageUrl,
                ImagesNames = farm.ImagesNames
            };

            return request;
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

        public async Task UpdateFarmImages(UpdateFarmImagesDto updateFarmImagesDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmImagesDto.FarmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {updateFarmImagesDto.FarmId} does not exist.");

            if (updateFarmImagesDto.Images == null)
            {
                await ImageHelper.DeleteImages(farm.ImagesNames, FarmsImageFolder);
                farm.ImagesNames = new List<string>();
            }
            else
            {
                await ImageHelper.DeleteImages(farm.ImagesNames, FarmsImageFolder);
                var imagesPaths = await ImageHelper.SaveImages(updateFarmImagesDto.Images, FarmsImageFolder);

                farm.ImagesNames = imagesPaths;
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateFarmCategoriesAndSubcategories(UpdateFarmCategoriesAndSubcategoriesDto updateFarmCategoriesAndSubcategoriesDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmCategoriesAndSubcategoriesDto.FarmId);
            if (farm == null) 
                throw new NotFoundException($"Farm with Id {updateFarmCategoriesAndSubcategoriesDto.FarmId} does not exist.");

            farm.Categories = updateFarmCategoriesAndSubcategoriesDto.Categories;
            farm.Subcategories = updateFarmCategoriesAndSubcategoriesDto.Subcategories;

            await DbContext.SaveChangesAsync();
        }
    }
}
