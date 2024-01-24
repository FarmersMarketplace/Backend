using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Farm;
using ProjectForFarmers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = SixLabors.ImageSharp.Image;
using Microsoft.AspNetCore.Http;
using System.Security.AccessControl;
using ProjectForFarmers.Application.Helpers;
using AutoMapper;

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

        public async Task Update(UpdateFarmDto updateFarmDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmDto.FarmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {updateFarmDto.FarmId} does not exist.");

            farm = Mapper.Map<Farm>(updateFarmDto);
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
                ImagesPaths = imagesPaths
            };

            return request;
        }

        public async Task<FarmListVm> GetAll(Guid userId)
        {
            var farms = DbContext.Farms.Where(f => f.OwnerId == userId).ToArray();
            var response = new FarmListVm();
            foreach ( var f in farms )
            {
                var vm = new FarmLookupVm(f.Id, f.Name);
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
