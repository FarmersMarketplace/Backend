using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Application.ViewModels.Product;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IProductService
    {
        Task Create(CreateProductDto createProductDto);
        Task Update(UpdateProductDto updateProductDto);
        Task Delete(Guid productId);
        Task<ProductVm> Get(Guid productId);
        Task<AllProductsVm> GetAll (Guid producerId, Producer producer);
        Task<ProductsListVm> GetFilteredProducts(Guid producerId, Producer producer, ProductFilter filter);
    }

}
