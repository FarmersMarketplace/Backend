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
using Geocoding;
using Geocoding.Google;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Address = ProjectForFarmers.Domain.Address;
using ProjectForFarmers.Application.Mappings;
using System.Net;

namespace ProjectForFarmers.Application.Services.Business
{
    public class FarmService : Service, IFarmService
    {
        private readonly string FarmsImageFolder;
        private readonly ImageHelper ImageHelper;

        public FarmService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            FarmsImageFolder = Configuration["Images:Farms"];
            ImageHelper = new ImageHelper();
        }

        public async Task Create(CreateFarmDto createFarmDto)
        {
            var farm = Mapper.Map<Farm>(createFarmDto);
            var address = farm.Address;

            var coords = await GetCoordinates(address);
            address.Latitude = coords.Latitude;
            address.Longtitude = coords.Longitude;

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
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmDto.FarmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {updateFarmDto.FarmId} does not exist.");

            var farmAddress = farm.Address;
            var addressDto = updateFarmDto.Address;
            var farmCreationDate = farm.CreationDate;

            farm = Mapper.Map<Farm>(updateFarmDto);

            if(farmAddress.Region != addressDto.Region
                || farmAddress.District != addressDto.District
                || farmAddress.Settlement != addressDto.Settlement
                || farmAddress.Street != addressDto.Street
                || farmAddress.HouseNumber != addressDto.HouseNumber)
            {
                var coords = await GetCoordinates(addressDto);
                farm.Address.Latitude = coords.Latitude;
                farm.Address.Longtitude = coords.Longitude;
            }

            farm.CreationDate = farmCreationDate;

            await DbContext.SaveChangesAsync();
        }

        public async Task<FarmVm> Get(Guid farmId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {farmId} does not exist.");

            var imagesPaths = farm.ImagesNames.Select(i => Path.Combine(FarmsImageFolder.Replace("wwwroot/", ""), i)).ToList();

            var request = new FarmVm
            {
                Id = farm.Id,
                Name = farm.Name,
                Description = farm.Description,
                ContactEmail = farm.ContactEmail,
                OwnerName = farm.Owner.Name + " " + farm.Owner.Surname,
                Address = farm.Address,
                WebsiteUrl = farm.WebsiteUrl,
                ImagesNames = imagesPaths
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
            if(farm == null) throw new NotFoundException($"Farm with Id {updateFarmImagesDto.FarmId} does not exist.");

            await ImageHelper.DeleteImages(farm.ImagesNames, FarmsImageFolder);
            var imagesPaths = await ImageHelper.SaveImages(updateFarmImagesDto.Images, FarmsImageFolder);

            farm.ImagesNames = imagesPaths;
            await DbContext.SaveChangesAsync();
        }

    }
}
