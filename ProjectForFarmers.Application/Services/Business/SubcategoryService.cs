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
        public SubcategoryService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }

        public async Task Create(CreateSubcategoryDto createSubcategoryDto)
        {
            var category = DbContext.Categories.FirstOrDefault(c => c.Id == createSubcategoryDto.CategoryId);

            if (category == null)
            {
                string message = $"Category with Id {createSubcategoryDto.CategoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", createSubcategoryDto.CategoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            Guid id = Guid.NewGuid();
            var subcategory = new Subcategory
            {
                Id = id,
                Name = createSubcategoryDto.Name,
                CategoryId = createSubcategoryDto.CategoryId
            };

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

        public async Task Update(UpdateSubcategoryDto updateSubcategoryDto)
        {
            var subcategory = DbContext.Categories.FirstOrDefault(c => c.Id == updateSubcategoryDto.Id);

            if (subcategory == null)
            {
                string message = $"Subcategory with Id {updateSubcategoryDto.Id} was not found.";
                string userFacingMessage = CultureHelper.Exception("SubcategoryWithIdNotFound", updateSubcategoryDto.Id.ToString());
                throw new NotFoundException(message, userFacingMessage);
            }

            subcategory.Name = updateSubcategoryDto.Name;

            await DbContext.SaveChangesAsync();
        }
    }

}
