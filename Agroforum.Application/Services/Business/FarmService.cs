using Agroforum.Application.DataTransferObjects.Farm;
using Agroforum.Application.Exceptions;
using Agroforum.Application.Interfaces;
using Agroforum.Application.ViewModels.Farm;
using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp.Formats.Jpeg;
using Image = SixLabors.ImageSharp.Image;

namespace Agroforum.Application.Services.Business
{
    public class FarmService : IFarmService
    {
        private IAgroforumDbContext DbContext { get; set; }
        private IConfiguration Configuration { get; set; }

        public FarmService(IAgroforumDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            Configuration = configuration;
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
                PostalCode = createFarmDto.PostalCode
            };

            var imagePaths = await AddFarmImages(createFarmDto.Images);

            var farm = new Farm
            {
                Id = Guid.NewGuid(),
                Name = createFarmDto.Name,
                Description = createFarmDto.Description,
                ContactEmail = createFarmDto.ContactEmail,
                OwnerId = createFarmDto.OwnerId,
                AddressId = address.Id,
                ImagePaths = imagePaths
            };

            await DbContext.Addresses.AddAsync(address);
            await DbContext.Farms.AddAsync(farm);
            await DbContext.SaveChangesAsync();
        }

        public async Task<List<string>> AddFarmImages(ICollection<byte[]>? images)
        {
            var imagePaths = new List<string>();

            foreach (var imageBytes in images)
            {
                var imageName = Guid.NewGuid().ToString() + ".jpg";
                var imagePath = Path.Combine(Configuration["Environment:ImagesPath"], imageName);

                using (var imageStream = new MemoryStream(imageBytes))
                {
                    using (var image = Image.Load(imageStream))
                    {
                        using (var outputStream = new MemoryStream())
                        {
                            image.Save(outputStream, new JpegEncoder());
                            await File.WriteAllBytesAsync(imagePath, outputStream.ToArray());
                        }
                    }
                }

                imagePaths.Add(imagePath);
            }

            return imagePaths;
        }

        public async Task Delete(DeleteFarmDto deleteFarmDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == deleteFarmDto.FarmId);
            if (farm != null) throw new NotFoundException($"Farm with Id {deleteFarmDto.FarmId} does not exist.");

            DbContext.Farms.Remove(farm);
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(UpdateFarmDto updateFarmDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmDto.FarmId);
            if (farm != null) throw new NotFoundException($"Farm with Id {updateFarmDto.FarmId} does not exist.");

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

        public async Task<FarmVm> Get(GetFarmDto getFarmDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == getFarmDto.FarmId);
            if (farm != null) throw new NotFoundException($"Farm with Id {getFarmDto.FarmId} does not exist.");
            var owner = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == farm.OwnerId);
            if(owner == null) throw new NotFoundException($"Farm with Id {getFarmDto.FarmId} does not have owner.");
            var address = await DbContext.Addresses.FirstOrDefaultAsync(f => f.Id == getFarmDto.FarmId);
            if (address == null)
            {
                address = new Address() { Id = Guid.NewGuid() };
                farm.AddressId = address.Id;
                DbContext.Addresses.Add(address);
            }

            var images = await GetFarmImages(farm.ImagePaths);

            var request = new FarmVm
            {
                Id = farm.Id,
                Name = farm.Name,
                Description = farm.Description,
                ContactEmail = farm.ContactEmail,
                OwnerName = owner.Name + " " + owner.Surname,
                Address = address,
                Images = images
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
            throw new NotImplementedException();
        }

        public async Task UpdateImages(UpdateFarmImagesDto updateFarmImagesDto)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == updateFarmImagesDto.FarmId);
            if (farm != null) throw new NotFoundException($"Farm with Id {updateFarmImagesDto.FarmId} does not exist.");

            await DeleteFarmImages(farm.ImagePaths);
            var paths = await AddFarmImages(updateFarmImagesDto.Images);
            farm.ImagePaths = paths;
            await DbContext.SaveChangesAsync();
        }

        private async Task DeleteFarmImages(List<string>? imagePaths)
        {
            foreach (var imagePath in imagePaths)
            {
                var fullPath = Path.Combine(Configuration["Environment:ImagesPath"], imagePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
        }
    }
}
