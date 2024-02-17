using ProjectForFarmers.Application.DataTransferObjects.Catefory;
using ProjectForFarmers.Application.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface ICategoryService
    {
        Task Create(CategoryDto createCategoryDto);
        Task Update(Guid categoryId, CategoryDto categoryDto);
        Task Delete(Guid categoryId);
        Task<CategoryListVm> GetAll();
    }
}
