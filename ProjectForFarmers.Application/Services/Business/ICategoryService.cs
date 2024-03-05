using FarmersMarketplace.Application.DataTransferObjects.Catefory;
using FarmersMarketplace.Application.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface ICategoryService
    {
        Task Create(CategoryDto createCategoryDto);
        Task Update(Guid categoryId, CategoryDto categoryDto);
        Task Delete(Guid categoryId);
        Task<CategoryListVm> GetAll();
    }
}
