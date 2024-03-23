using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FarmersMarketplace.Application.Services.Business
{
    public class AccountService : Service, IAccountService
    {
        public AccountService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
        }

        public Task DeleteAccount(Role role, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomerVm> GetCustomer(Guid accountId)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == accountId);

            if(customer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            var vm = Mapper.Map<CustomerVm>(customer);

            return vm;
        }

        public async Task<FarmerVm> GetFarmer(Guid accountId)
        {
            var farmer = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (farmer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            var vm = Mapper.Map<FarmerVm>(farmer);

            return vm;
        }

        public async Task<SellerVm> GetSeller(Guid accountId)
        {
            var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (seller == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            var vm = Mapper.Map<SellerVm>(seller);

            return vm;
        }

        public Task UpdateCustomer(UpdateCustomerDto dto, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCustomerPaymentData(CustomerPaymentDataDto dto, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFarmer(UpdateFarmerDto dto, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFarmerPaymentData(FarmerPaymentDataDto dto, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSeller(UpdateSellerDto dto, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSellerCategoriesAndSubcategories(SellerCategoriesAndSubcategoriesDto dto, Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateSellerPaymentData(ProducerPaymentDataDto dto, Guid accountId)
        {
            throw new NotImplementedException();
        }
    }
}
