using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.DataTransferObjects.Catefory;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface ICategoryService
    {
        Task Create(CategoryDto dto);
        Task Update(Guid categoryId, CategoryDto dto);
        Task Delete(Guid categoryId);
        Task<CategoryListVm> GetAll();
        Task<CategoriesAndSubcategoriesVm> GetProducerData(Guid producerId, Producer producer);
    }
}
