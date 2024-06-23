using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.DataTransferObjects.Producers;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IAccountService
    {
        Task<CustomerVm> GetCustomer(Guid accountId);
        Task UpdateCustomer(UpdateCustomerDto dto, Guid accountId);
        Task UpdateCustomerPaymentData(CustomerPaymentDataDto dto, Guid accountId);
        Task<SellerVm> GetSeller(Guid accountId);
        Task UpdateSeller(UpdateSellerDto dto, Guid accountId);
        Task UpdateSellerCategoriesAndSubcategories(SellerCategoriesAndSubcategoriesDto dto, Guid accountId);
        Task<FarmerVm> GetFarmer(Guid accountId);
        Task UpdateFarmer(UpdateFarmerDto dto, Guid accountId);
        Task UpdateFarmerPaymentData(ProducerPaymentDataDto dto, Guid accountId);
        Task UpdateSellerPaymentData(ProducerPaymentDataDto dto, Guid accountId);
        Task DeleteAccount(Role role, Guid accountId);
        Task<CustomerOrderDetailsVm> GetCustomerOrderDetails(Guid accountId, ReceivingMethod receivingMethod);
        Task<SellerForCustomerVm> GetSellerForCustomer(Guid accountId);
    }
}
