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

namespace ProjectForFarmers.Application.Services.Business
{
    public class FarmService : IFarmService
    {
        private readonly IApplicationDbContext DbContext;
        private IConfiguration Configuration { get; set; }

        public FarmService(IApplicationDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            Configuration = configuration;
        }

        public async Task Create(CreateFarmDto createFarmDto)
        {
            //throw new Exception("Solve images problem!");
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

            var imagesPaths = await HandleFarmImages(createFarmDto.Images);

            var farm = new Farm
            {
                Id = Guid.NewGuid(),
                Name = createFarmDto.Name,
                Description = createFarmDto.Description,
                ContactEmail = createFarmDto.ContactEmail,
                IsVisibleOnMap = createFarmDto.IsVisibleOnMap,
                //ImagesPaths = imagesPaths
                OwnerId = createFarmDto.OwnerId,
                AddressId = address.Id,
            };

            await DbContext.Addresses.AddAsync(address);
            await DbContext.Farms.AddAsync(farm);
            await DbContext.SaveChangesAsync();
        }

        public async Task<List<string>> HandleFarmImages(List<IFormFile> images)
        {
            var imagePaths = new List<string>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString("N");
                    var filePath = Directory.GetCurrentDirectory();

                    using (var stream = new FileStream(fileName, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    imagePaths.Add(fileName);
                }
            }

            return imagePaths;
        }

        public async Task Delete(Guid farmId, Guid ownerId)
        {
            throw new Exception("Solve images problem!");
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == farmId && f.OwnerId == ownerId);
            if (farm == null) throw new NotFoundException($"Farm with Id {farmId} does not exist.");

            DbContext.Farms.Remove(farm);
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(UpdateFarmDto updateFarmDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmDto.FarmId);
            if (farm == null) throw new NotFoundException($"Farm with Id {updateFarmDto.FarmId} does not exist.");

            var address = await DbContext.Addresses.FirstOrDefaultAsync(f => f.Id == updateFarmDto.FarmId);
            if (address == null)
            {
                address = new Address() { Id = Guid.NewGuid() };
                farm.AddressId = address.Id;
                DbContext.Addresses.Add(address);
            }

            farm.Name = updateFarmDto.Name;
            farm.Description = updateFarmDto.Description;
            farm.ContactEmail = updateFarmDto.ContactEmail;

            address.Region = updateFarmDto.Region;
            address.Settlement = updateFarmDto.Settlement;
            address.Street = updateFarmDto.Street;
            address.HouseNumber = updateFarmDto.HouseNumber;
            address.PostalCode = updateFarmDto.PostalCode;

            await DbContext.SaveChangesAsync();
        }

        public async Task<FarmVm> Get(Guid farmId)
        {
            throw new Exception("Solve images problem!");
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

            var request = new FarmVm
            {
                Id = farm.Id,
                Name = farm.Name,
                Description = farm.Description,
                ContactEmail = farm.ContactEmail,
                OwnerName = owner.Name + " " + owner.Surname,
                Address = address,
            };

            return request;
        }

        private async Task<List<byte[]>> GetFarmImages(List<string> imagePaths)
        {
            var imageBytesList = new List<byte[]>();

            foreach (var imagePath in imagePaths)
            {
                var imageBytes = await File.ReadAllBytesAsync(Path.Combine(Configuration["Environment:ImagesPath"], imagePath));
                imageBytesList.Add(imageBytes);
            }

            return imageBytesList;
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

        public Task UpdateFarmImages(UpdateFarmImagesDto updateFarmImagesDto)
        {
            throw new NotImplementedException();
        }

        //private async Task DeleteImages(List<string>? imagePaths)
        //{
        //    foreach (var imagePath in imagePaths)
        //    {
        //        var fullPath = Path.Combine(Configuration["Environment:ImagesPath"], imagePath);
        //        if (File.Exists(fullPath))
        //        {
        //            File.Delete(fullPath);
        //        }
        //    }
        //}
    }
}
