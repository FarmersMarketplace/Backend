using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Catefory;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FarmersMarketplace.Application.Services.Business
{
    public class CategoryService : Service, ICategoryService
    {
        public CategoryService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }

        public async Task Create(CategoryDto dto)
        {
            Guid id = Guid.NewGuid();
            var category = new Category 
            { 
                Id = id,
                Name = dto.Name
            };

            DbContext.Categories.Add(category);
            await DbContext.SaveChangesAsync();
        }

        public async Task Delete(Guid categoryId)
        {
            var category = DbContext.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                string message = $"Category with Id {categoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", categoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            DbContext.Categories.Remove(category);
            await DbContext.SaveChangesAsync();
        }

        public async Task<CategoryListVm> GetAll()
        {
            var categories = DbContext.Categories.Include(c => c.Subcategories).ToList();

            var vm = new CategoryListVm();

            foreach (var category in categories)
            {
                var categoryVm = Mapper.Map<CategoryVm>(category);

                vm.Categories.Add(categoryVm);
            }

            return vm;
        }

        public async Task Update(Guid categoryId, CategoryDto dto)
        {
            var category = DbContext.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category != null)
            {
                string message = $"Category with Id {categoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", categoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            category.Name = dto.Name;

            await DbContext.SaveChangesAsync();
        }
    }

}
