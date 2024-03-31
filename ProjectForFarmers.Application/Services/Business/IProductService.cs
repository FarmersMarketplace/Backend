﻿using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IProductService
    {
        Task Create(CreateProductDto dto);
        Task Update(UpdateProductDto dto, Guid accountId);
        Task Delete(ProductListDto dto, Guid accountId);
        Task ChangeStatus(ProductListDto dto, ProductStatus status, Guid accountId);
        Task Duplicate(ProductListDto dto, Guid accountId);
        Task<ProducerProductVm> Get(Guid productId);
        Task<ProducerProductListVm> GetAll (GetProducerProductListDto dto);
        Task<OptionListVm> Autocomplete(ProductAutocompleteDto dto);
        Task<FilterData> GetFilterData(Producer producer, Guid producerId);
        Task<(string fileName, byte[] bytes)> ExportToExcel(ExportProductsDto dto);
    }

}
