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
                throw new NotFoundException(message, "CategoryWithIdNotFound", categoryId.ToString());
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

        public async Task<CategoriesAndSubcategoriesVm> GetProducerData(Guid producerId, Producer producer)
        {
            if (producer == Producer.Seller)
            {
                return await GetSellerData(producerId);
            }
            else if (producer == Producer.Farm)
            {
                return await GetFarmData(producerId);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private async Task<CategoriesAndSubcategoriesVm> GetFarmData(Guid producerId)
        {
            var vm = new CategoriesAndSubcategoriesVm();

            var farm = await DbContext.Farms.FirstOrDefaultAsync(f => f.Id == producerId);

            if (farm == null)
            {
                string message = $"Farm with Id {producerId} was not found.";
                throw new NotFoundException(message, "FarmNotFound");
            }

            foreach (var categoryId in farm.Categories)
            {
                var category = DbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category == null)
                {
                    string message = $"Category with Id {categoryId} was not found.";
                    throw new NotFoundException(message, "CategoryNotFound");
                }

                vm.Categories.Add(new CategoryLookupVm(category.Id, category.Name));
            }

            foreach (var subcategoryId in farm.Subcategories)
            {
                var subcategory = DbContext.Subcategories.FirstOrDefault(c => c.Id == subcategoryId);
                if (subcategory == null)
                {
                    string message = $"Subcategory with Id {subcategoryId} was not found.";
                    throw new NotFoundException(message, "SubcategoryNotFound");
                }

                vm.Subcategories.Add(new SubcategoryVm(subcategory.Id, subcategory.Name, subcategory.CategoryId));
            }

            return vm;
        }

        private async Task<CategoriesAndSubcategoriesVm> GetSellerData(Guid producerId)
        {
            var vm = new CategoriesAndSubcategoriesVm();

            var seller = await DbContext.Sellers.FirstOrDefaultAsync(s => s.Id == producerId);

            if (seller == null)
            {
                string message = $"Account with Id {producerId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            foreach (var categoryId in seller.Categories)
            {
                var category = DbContext.Categories.FirstOrDefault(c => c.Id == categoryId);
                if (category == null)
                {
                    string message = $"Category with Id {categoryId} was not found.";
                    throw new NotFoundException(message, "CategoryNotFound");
                }

                vm.Categories.Add(new CategoryLookupVm(category.Id, category.Name));
            }

            foreach (var subcategoryId in seller.Subcategories)
            {
                var subcategory = DbContext.Subcategories.FirstOrDefault(c => c.Id == subcategoryId);
                if (subcategory == null)
                {
                    string message = $"Subcategory with Id {subcategoryId} was not found.";
                    throw new NotFoundException(message, "SubcategoryNotFound");
                }

                vm.Subcategories.Add(new SubcategoryVm(subcategory.Id, subcategory.Name, subcategory.CategoryId));
            }

            return vm;
        }

        public async Task Update(Guid categoryId, CategoryDto dto)
        {
            var category = DbContext.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category != null)
            {
                string message = $"Category with Id {categoryId} was not found.";
                throw new NotFoundException(message, "CategoryWithIdNotFound", categoryId.ToString());
            }

            category.Name = dto.Name;

            await DbContext.SaveChangesAsync();
        }
    }

}
