using AutoMapper;
using Microsoft.Extensions.Configuration;
using FarmersMarketplace.Application.DataTransferObjects.Subcategory;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Services.Business
{
    public class SubcategoryService : Service, ISubcategoryService
    {
        private readonly FileHelper FileHelper;

        public SubcategoryService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            FileHelper = new FileHelper();
        }

        public async Task Create(CreateSubcategoryDto dto)
        {
            var category = DbContext.Categories.FirstOrDefault(c => c.Id == dto.CategoryId);

            if (category == null)
            {
                string message = $"Category with Id {dto.CategoryId} was not found.";
                throw new NotFoundException(message, "CategoryWithIdNotFound", dto.CategoryId.ToString());
            }

            Guid id = Guid.NewGuid();
            var subcategory = new Subcategory
            {
                Id = id,
                Name = dto.Name,
                CategoryId = dto.CategoryId
            };

            if (!FileHelper.IsValidImage(dto.Image))
            {
                string acceptableFormats = string.Join(", ", FileHelper.AllowedImagesExtensions);
                string message = $"Invalid format of image {dto.Image.FileName}. Acceptable formats: {acceptableFormats}.";
                throw new NotFoundException(message, "InvalidImageFormat", dto.Image.FileName, acceptableFormats);
            }

            subcategory.ImageName = await FileHelper.SaveFile(dto.Image, Configuration["File:Images:Product"]);

            DbContext.Subcategories.Add(subcategory);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid subcategoryId)
        {
            var subcategory = DbContext.Categories.FirstOrDefault(c => c.Id == subcategoryId);

            if (subcategory == null)
            {
                string message = $"Subcategory with Id {subcategoryId} was not found.";
                throw new NotFoundException(message, "SubcategoryWithIdNotFound", subcategoryId.ToString());
            }

            DbContext.Categories.Remove(subcategory);
            await DbContext.SaveChangesAsync();
        }

        public async Task Update(UpdateSubcategoryDto dto)
        {
            var subcategory = DbContext.Categories.FirstOrDefault(c => c.Id == dto.Id);

            if (subcategory == null)
            {
                string message = $"Subcategory with Id {dto.Id} was not found.";
                throw new NotFoundException(message, "SubcategoryWithIdNotFound", dto.Id.ToString());
            }

            subcategory.Name = dto.Name;

            await DbContext.SaveChangesAsync();
        }
    }

}
