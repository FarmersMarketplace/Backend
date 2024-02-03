using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IProductService
    {
        Task Create(ProductDto productDto);
        Task Update(Guid productId, ProductDto productDto);
        Task Delete(Guid productId);
        Task<ProductVm> Get(Guid productId);
        Task GetAll (Guid producerId, Producer producer, ProductFilter filter);
    }

}
