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
using Address = FarmersMarketplace.Domain.Address;

namespace FarmersMarketplace.Application.Services.Business
{
    public class AccountService : Service, IAccountService
    {
        private readonly CoordinateHelper CoordinateHelper;
        private readonly FileHelper FileHelper;

        public AccountService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            CoordinateHelper = new CoordinateHelper(configuration);
            FileHelper = new FileHelper();
        }

        public async Task DeleteAccount(Role role, Guid accountId)
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

        public async Task UpdateCustomer(UpdateCustomerDto dto, Guid accountId)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (customer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            customer.Name = dto.Name;
            customer.Surname = dto.Surname;
            customer.Patronymic = dto.Patronymic;
            customer.Phone = dto.Phone;
            customer.Gender = dto.Gender;
            customer.DateOfBirth = dto.DateOfBirth;

            if (dto.Avatar != null && customer.AvatarName != dto.Avatar.Name)
            {
                customer.AvatarName = await FileHelper.SaveFile(dto.Avatar, Configuration["File:Images:Account"]);
            }

            if (dto.Address != null)
            {
                var addressDto = dto.Address;
                if (customer.Address == null)
                {
                    customer.Address = new CustomerAddress(); 
                }

                if(AddressEqualToDto(customer.Address, addressDto))
                {
                    customer.Address.Region = addressDto.Region;
                    customer.Address.District = addressDto.District;
                    customer.Address.Settlement = addressDto.Settlement;
                    customer.Address.Street = addressDto.Street;
                    customer.Address.HouseNumber = addressDto.HouseNumber;
                    customer.Address.PostalCode = addressDto.PostalCode;
                    customer.Address.Apartment = addressDto.Apartment;
                    customer.Address.Note = addressDto.Note;

                    var coords = await CoordinateHelper.GetCoordinates(customer.Address);
                    customer.Address.Latitude = coords.Latitude;
                    customer.Address.Longitude = coords.Longitude;
                }
            }

            await DbContext.SaveChangesAsync();
        }

        public bool AddressEqualToDto(Address address, AddressDto dto)
        {
            return address.Region == dto.Region &&
                    address.District == dto.District &&
                    address.Settlement == dto.Settlement &&
                    address.Street == dto.Street &&
                    address.HouseNumber == dto.HouseNumber &&
                    address.PostalCode == dto.PostalCode;
        }

        public async Task UpdateCustomerPaymentData(CustomerPaymentDataDto dto, Guid accountId)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (customer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            if (customer.PaymentData == null)
            {
                customer.PaymentData = new CustomerPaymentData();
            }

            customer.PaymentData.CardNumber = dto.CardNumber;
            customer.PaymentData.CardExpirationYear = dto.CardExpirationYear;
            customer.PaymentData.CardExpirationMonth = dto.CardExpirationMonth;

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateFarmer(UpdateFarmerDto dto, Guid accountId)
        {
            var farmer = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (farmer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            farmer.Name = dto.Name;
            farmer.Surname = dto.Surname;
            farmer.Patronymic = dto.Patronymic;
            farmer.Phone = dto.Phone;
            farmer.Gender = dto.Gender;
            farmer.DateOfBirth = dto.DateOfBirth;

            if (dto.Avatar != null && farmer.AvatarName != dto.Avatar.Name)
            {
                farmer.AvatarName = await FileHelper.SaveFile(dto.Avatar, Configuration["File:Images:Account"]);
            }

            if (dto.Address != null)
            {
                var addressDto = dto.Address;
                farmer.Address = farmer.Address ?? new Address();

                if (!AddressEqualToDto(farmer.Address, addressDto))
                {
                    farmer.Address.Region = addressDto.Region;
                    farmer.Address.District = addressDto.District;
                    farmer.Address.Settlement = addressDto.Settlement;
                    farmer.Address.Street = addressDto.Street;
                    farmer.Address.HouseNumber = addressDto.HouseNumber;
                    farmer.Address.PostalCode = addressDto.PostalCode;

                    var coords = await CoordinateHelper.GetCoordinates(farmer.Address);
                    farmer.Address.Latitude = coords.Latitude;
                    farmer.Address.Longitude = coords.Longitude;
                }
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateSeller(UpdateSellerDto dto, Guid accountId)
        {
            var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (seller == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            seller.Name = dto.Name;
            seller.Surname = dto.Surname;
            seller.Patronymic = dto.Patronymic;
            seller.Phone = dto.Phone;
            seller.Gender = dto.Gender;
            seller.DateOfBirth = dto.DateOfBirth;

            if (dto.Images != null)
            {
                var imageNames = await FileHelper.SaveImages(dto.Images, Configuration["File:Images:Account"]);
            }

            if (dto.Address != null)
            {
                var addressDto = dto.Address;
                seller.Address = seller.Address ?? new Address();

                if (!AddressEqualToDto(seller.Address, addressDto))
                {
                    seller.Address.Region = addressDto.Region;
                    seller.Address.District = addressDto.District;
                    seller.Address.Settlement = addressDto.Settlement;
                    seller.Address.Street = addressDto.Street;
                    seller.Address.HouseNumber = addressDto.HouseNumber;
                    seller.Address.PostalCode = addressDto.PostalCode;

                    var coords = await CoordinateHelper.GetCoordinates(seller.Address);
                    seller.Address.Latitude = coords.Latitude;
                    seller.Address.Longitude = coords.Longitude;
                }
            }

            if (dto.Schedule != null)
            {
                await UpdateSellerSchedule(seller, dto.Schedule);
            }

            seller.FirstSocialPageUrl = dto.FirstSocialPageUrl;
            seller.SecondSocialPageUrl = dto.SecondSocialPageUrl;

            await DbContext.SaveChangesAsync();
        }

        private async Task UpdateSellerSchedule(Seller seller, ScheduleDto dto)
        {
            var schedule = seller.Schedule ?? new Schedule(); 

            schedule.Monday.IsOpened = dto.Monday.IsOpened;
            schedule.Monday.StartHour = dto.Monday.StartHour;
            schedule.Monday.StartMinute = dto.Monday.StartMinute;
            schedule.Monday.EndHour = dto.Monday.EndHour;
            schedule.Monday.EndMinute = dto.Monday.EndMinute;

            seller.Schedule = schedule;
        }


        public async Task UpdateSellerCategoriesAndSubcategories(SellerCategoriesAndSubcategoriesDto dto, Guid accountId)
        {
            var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (seller == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            seller.Categories = dto.Categories ?? seller.Categories;
            seller.Subcategories = dto.Subcategories ?? seller.Subcategories;

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateProducerPaymentData(ProducerPaymentDataDto dto, Guid accountId, Role producer)
        {
            if (producer == Role.Seller)
            {
                var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == accountId);

                if (seller == null)
                {
                    string message = $"Account with Id {accountId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                    throw new NotFoundException(message, userFacingMessage);
                }

                seller.PaymentData.CardNumber = dto.CardNumber;
                seller.PaymentData.AccountNumber = dto.AccountNumber;
                seller.PaymentData.BankUSREOU = dto.BankUSREOU;
                seller.PaymentData.BIC = dto.BIC;
                seller.PaymentData.HolderFullName = dto.HolderFullName;
                seller.PaymentData.CardExpirationYear = dto.CardExpirationYear;
                seller.PaymentData.CardExpirationMonth = dto.CardExpirationMonth;

                await DbContext.SaveChangesAsync();
            }
            else if (producer == Role.Farmer)
            {
                var farmer = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Id == accountId);

                if (farmer == null)
                {
                    string message = $"Account with Id {accountId} was not found.";
                    string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                    throw new NotFoundException(message, userFacingMessage);
                }

                farmer.PaymentData.CardNumber = dto.CardNumber;
                farmer.PaymentData.AccountNumber = dto.AccountNumber;
                farmer.PaymentData.BankUSREOU = dto.BankUSREOU;
                farmer.PaymentData.BIC = dto.BIC;
                farmer.PaymentData.HolderFullName = dto.HolderFullName;
                farmer.PaymentData.CardExpirationYear = dto.CardExpirationYear;
                farmer.PaymentData.CardExpirationMonth = dto.CardExpirationMonth;

                await DbContext.SaveChangesAsync();
            }
            else 
            {
                throw new NotImplementedException("There are no customers any more.");
            }
        }
    }
}
