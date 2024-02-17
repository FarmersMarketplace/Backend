using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Catefory;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Helpers;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Category;
using ProjectForFarmers.Application.ViewModels.Subcategory;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.Services.Business
{
    public class CategoryService : Service, ICategoryService
    {
        public CategoryService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }

        public async Task Create(CategoryDto createCategoryDto)
        {
            Guid id = Guid.NewGuid();
            var category = new Category 
            { 
                Id = id,
                Name = createCategoryDto.Name
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
                foreach (var subcategory in category.Subcategories)
                {
                    categoryVm.Subcategories.Add(Mapper.Map<SubcategoryVm>(subcategory));
                }

                vm.Categories.Add(categoryVm);
            }

            return vm;
        }

        public async Task Update(Guid categoryId, CategoryDto categoryDto)
        {
            var category = DbContext.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category != null)
            {
                string message = $"Category with Id {categoryId} was not found.";
                string userFacingMessage = CultureHelper.Exception("CategoryWithIdNotFound", categoryId.ToString());

                throw new NotFoundException(message, userFacingMessage);
            }

            category.Name = categoryDto.Name;

            await DbContext.SaveChangesAsync();
        }
    }

}
