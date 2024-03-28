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
                string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", dto.CategoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
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
                string userFacingMessage = CultureHelper.Exception("InvalidImageFormat", dto.Image.FileName, acceptableFormats);
                throw new NotFoundException(message, userFacingMessage);
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
                string userFacingMessage = CultureHelper.Exception("SubcategoryWithIdNotFound", subcategoryId.ToString());
                throw new NotFoundException(message, userFacingMessage);
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
                string userFacingMessage = CultureHelper.Exception("SubcategoryWithIdNotFound", dto.Id.ToString());
                throw new NotFoundException(message, userFacingMessage);
            }

            subcategory.Name = dto.Name;

            await DbContext.SaveChangesAsync();
        }
    }

}
