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

namespace ProjectForFarmers.Application.Services.Business
{
    public class FarmService : IFarmService
    {
        private readonly IApplicationDbContext DbContext;
        private IConfiguration Configuration { get; set; }
        private readonly string FarmsImageFolder;
        private readonly ImageHelper ImageService;

        public FarmService(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            Configuration = configuration;
            FarmsImageFolder = Configuration["Images:Farms"];
            ImageService = new ImageHelper();

        }

        public async Task Create(CreateFarmDto createFarmDto)
        {
            var address = new Address
            {
                Id = Guid.NewGuid(),
                Region = createFarmDto.Region,
                Settlement = createFarmDto.Settlement,
                Street = createFarmDto.Street,
                HouseNumber = createFarmDto.HouseNumber,
                PostalCode = createFarmDto.PostalCode,
                Note = createFarmDto.Note
            };

            var schedule = new Schedule
            {
                Id = Guid.NewGuid(),
                
            };

            var imagesPaths = await ImageService.SaveImages(createFarmDto.Images, FarmsImageFolder);

            var farm = new Farm
            {
                Id = Guid.NewGuid(),
                Name = createFarmDto.Name,
                Description = createFarmDto.Description,
                ContactEmail = createFarmDto.ContactEmail,
                ImagesNames = imagesPaths,
                OwnerId = createFarmDto.OwnerId,
                AddressId = address.Id,
            };

            await DbContext.Addresses.AddAsync(address);
            await DbContext.Farms.AddAsync(farm);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid farmId, Guid ownerId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == farmId && f.OwnerId == ownerId);
            if (farm == null) throw new NotFoundException($"Farm with Id {farmId} does not exist.");
            DbContext.Farms.Remove(farm);
            await ImageService.DeleteImages(farm.ImagesNames, FarmsImageFolder);
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(UpdateFarmDto updateFarmDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmDto.FarmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {updateFarmDto.FarmId} does not exist.");

            farm.Name = updateFarmDto.Name;
            farm.Description = updateFarmDto.Description;
            farm.ContactEmail = updateFarmDto.ContactEmail;
            farm.ContactPhone = updateFarmDto.ContactPhone;
            farm.WebsiteUrl = updateFarmDto.WebsiteUrl;

            farm.Address.Region = updateFarmDto.Region;
            farm.Address.Settlement = updateFarmDto.Settlement;
            farm.Address.Street = updateFarmDto.Street;
            farm.Address.HouseNumber = updateFarmDto.HouseNumber;
            farm.Address.PostalCode = updateFarmDto.PostalCode;
            farm.Address.Note = updateFarmDto.Note;

            await DbContext.SaveChangesAsync();
        }

        public async Task<FarmVm> Get(Guid farmId)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == farmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {farmId} does not exist.");

            var owner = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == farm.OwnerId);
            if(owner == null) throw new NotFoundException($"Farm with Id {farmId} does not have owner.");

            var address = await DbContext.Addresses.FirstOrDefaultAsync(f => f.Id == farmId);
            if (address == null)
            {
                address = new Address() { Id = Guid.NewGuid() };
                farm.AddressId = address.Id;
                DbContext.Addresses.Add(address);
            }

            var imagesPaths = farm.ImagesNames.Select(i => Path.Combine(FarmsImageFolder.Replace("wwwroot/", ""), i)).ToList();

            var request = new FarmVm
            {
                Id = farm.Id,
                Name = farm.Name,
                Description = farm.Description,
                ContactEmail = farm.ContactEmail,
                OwnerName = owner.Name + " " + owner.Surname,
                Address = address,
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

            await ImageService.DeleteImages(farm.ImagesNames, FarmsImageFolder);
            var imagesPaths = await ImageService.SaveImages(updateFarmImagesDto.Images, FarmsImageFolder);

            farm.ImagesNames = imagesPaths;
            await DbContext.SaveChangesAsync();
        }

    }
}
