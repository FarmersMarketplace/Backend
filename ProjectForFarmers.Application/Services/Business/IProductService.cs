using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.ViewModels.Dashboard;
using ProjectForFarmers.Application.ViewModels.Product;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IProductService
    {
        Task Create(CreateProductDto createProductDto);
        Task Update(UpdateProductDto updateProductDto, Guid accountId);
        Task Delete(ProductListDto productListDto, Guid accountId);
        Task Duplicate(ProductListDto productListDto, Guid accountId);
        Task<ProductVm> Get(Guid productId);
        Task<ProductListVm> GetAll (GetProductListDto getProductListDto);
        Task<OptionListVm> Autocomplete(ProductAutocompleteDto productAutocompleteDto);
        Task<FilterData> GetFilterData(Producer producer, Guid producerId);
        Task<(string fileName, byte[] bytes)> ExportToExcel(ExportProductsDto exportProductsDto);
    }

}
