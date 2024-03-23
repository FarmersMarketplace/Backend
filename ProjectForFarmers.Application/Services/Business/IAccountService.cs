using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Farm;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IAccountService
    {
        Task<CustomerVm> GetCustomer(Guid accountId);
        Task UpdateCustomer(UpdateCustomerDto dto);
        Task UpdateCustomerPaymentData(CustomerPaymentDataDto dto, Guid accountId);
        Task DeleteAccount(DeleteAccountDto dto);
        Task<SellerVm> GetSeller(Guid accountId);
        Task UpdateSeller(UpdateSellerDto dto);
        Task UpdateProducerPaymentData(UpdateProducerPaymentDataDto dto, Guid accountId);
        Task UpdateSellerCategoriesAndSubcategories(SellerCategoriesAndSubcategoriesDto dto);
        Task UpdateFarmer(UpdateFarmerDto dto);
    }
}
