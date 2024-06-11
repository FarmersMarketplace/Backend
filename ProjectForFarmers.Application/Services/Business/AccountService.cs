using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.DataTransferObjects.Producers;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

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
            var customer = await DbContext.Customers.Include(c => c.Address)
                .Include(c => c.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (customer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            var vm = Mapper.Map<CustomerVm>(customer);

            return vm;
        }

        public async Task<FarmerVm> GetFarmer(Guid accountId)
        {
            var farmer = await DbContext.Farmers.Include(c => c.Address)
                .Include(c => c.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (farmer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            var vm = Mapper.Map<FarmerVm>(farmer);

            if (farmer.PaymentTypes != null
                && farmer.PaymentTypes.Contains(PaymentType.Online))
            {
                vm.PaymentData.HasOnlinePayment = true;
            }
            else
            {
                vm.PaymentData.HasOnlinePayment = false;
            }

            return vm;
        }

        public async Task<SellerVm> GetSeller(Guid accountId)
        {
            var seller = await DbContext.Sellers.Include(c => c.Address)
                .Include(c => c.PaymentData)
                .Include(c => c.Schedule)
                    .ThenInclude(s => s.Monday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Tuesday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Wednesday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Thursday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Friday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Sunday)
                .Include(f => f.Schedule)
                    .ThenInclude(s => s.Saturday)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (seller == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            var vm = Mapper.Map<SellerVm>(seller);

            if (seller.PaymentTypes != null
                && seller.PaymentTypes.Contains(PaymentType.Online))
            {
                vm.PaymentData.HasOnlinePayment = true;
            }
            else
            {
                vm.PaymentData.HasOnlinePayment = false;
            }

            return vm;
        }

        public async Task UpdateCustomer(UpdateCustomerDto dto, Guid accountId)
        {
            var customer = await DbContext.Customers.Include(c => c.Address)
                .Include(c => c.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (customer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            customer.Name = dto.Name;
            customer.Surname = dto.Surname;
            customer.Patronymic = dto.Patronymic;
            customer.Phone = dto.Phone;
            customer.Gender = dto.Gender;
            customer.DateOfBirth = dto.DateOfBirth;

            if (dto.Avatar != null && customer.AvatarName != dto.Avatar.Name)
            {
                var oldAvatarName = customer.AvatarName;
                customer.AvatarName = await FileHelper.SaveFile(dto.Avatar, Configuration["File:Images:Account"]);

                if (!oldAvatarName.IsNullOrEmpty())
                    FileHelper.DeleteFile(oldAvatarName, Configuration["File:Images:Account"]);
            }

            if (dto.Address != null)
            {
                var addressDto = dto.Address;
                if (customer.Address == null)
                {
                    customer.Address = new CustomerAddress() { Id = Guid.NewGuid() };
                }

                customer.Address.Apartment = addressDto.Apartment;
                customer.Address.Note = addressDto.Note;
                customer.Address.PostalCode = addressDto.PostalCode;

                if (!AddressEqualToDto(customer.Address, addressDto))
                {
                    customer.Address.Region = addressDto.Region;
                    customer.Address.District = addressDto.District;
                    customer.Address.Settlement = addressDto.Settlement;
                    customer.Address.Street = addressDto.Street;
                    customer.Address.HouseNumber = addressDto.HouseNumber;

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
                    address.HouseNumber == dto.HouseNumber;
        }

        public async Task UpdateCustomerPaymentData(CustomerPaymentDataDto dto, Guid accountId)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (customer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            if (customer.PaymentData == null)
            {
                customer.PaymentData = new CustomerPaymentData() { Id = Guid.NewGuid() };
                DbContext.CustomerPaymentData.Add(customer.PaymentData);
            }

            customer.PaymentData.CardNumber = dto.CardNumber;
            customer.PaymentData.CardExpirationYear = dto.CardExpirationYear;
            customer.PaymentData.CardExpirationMonth = dto.CardExpirationMonth;

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateFarmer(UpdateFarmerDto dto, Guid accountId)
        {
            var farmer = await DbContext.Farmers.Include(f => f.Address)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (farmer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            if (farmer.Address == null)
            {
                farmer.Address = new Address() { Id = Guid.NewGuid() };
                DbContext.ProducerAddresses.Add(farmer.Address);
            }

            farmer.Name = dto.Name;
            farmer.Surname = dto.Surname;
            farmer.Patronymic = dto.Patronymic;
            farmer.Phone = dto.Phone;
            farmer.Gender = dto.Gender;
            farmer.DateOfBirth = dto.DateOfBirth.ToUniversalTime();
            farmer.AdditionalPhone = dto.AdditionalPhone;

            if (dto.Avatar != null && farmer.AvatarName != dto.Avatar.Name)
            {
                var oldAvatarName = farmer.AvatarName;
                farmer.AvatarName = await FileHelper.SaveFile(dto.Avatar, Configuration["File:Images:Account"]);

                if (!oldAvatarName.IsNullOrEmpty())
                    FileHelper.DeleteFile(oldAvatarName, Configuration["File:Images:Account"]);
            }

            if (dto.Address != null)
            {
                var addressDto = dto.Address;
                farmer.Address = farmer.Address ?? new Address();
                farmer.Address.Note = dto.Address.Note;
                farmer.Address.PostalCode = addressDto.PostalCode;

                if (!AddressEqualToDto(farmer.Address, addressDto))
                {
                    farmer.Address.Region = addressDto.Region;
                    farmer.Address.District = addressDto.District;
                    farmer.Address.Settlement = addressDto.Settlement;
                    farmer.Address.Street = addressDto.Street;
                    farmer.Address.HouseNumber = addressDto.HouseNumber;

                    var coords = await CoordinateHelper.GetCoordinates(farmer.Address);
                    farmer.Address.Latitude = coords.Latitude;
                    farmer.Address.Longitude = coords.Longitude;
                }
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateSeller(UpdateSellerDto dto, Guid accountId)
        {
            var seller = await DbContext.Sellers.Include(c => c.Schedule)
                .Include(c => c.Address)
                .Include(c => c.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (seller == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            seller.Name = dto.Name;
            seller.Surname = dto.Surname;
            seller.Patronymic = dto.Patronymic;
            seller.Phone = dto.Phone;
            seller.AdditionalPhone = dto.AdditionalPhone;
            seller.Gender = dto.Gender;
            seller.ReceivingMethods = dto.ReceivingMethods;
            seller.DateOfBirth = dto.DateOfBirth.ToUniversalTime();

            await UpdateSellerImages(seller, dto.Images);

            if (dto.Address != null)
            {
                var addressDto = dto.Address;

                if (seller.Address == null)
                {
                    seller.Address = new Address() { Id = Guid.NewGuid() };
                    DbContext.ProducerAddresses.Add(seller.Address);
                }

                seller.Address.PostalCode = addressDto.PostalCode;
                seller.Address.Note = addressDto.Note;

                if (!AddressEqualToDto(seller.Address, addressDto))
                {
                    seller.Address.Region = addressDto.Region;
                    seller.Address.District = addressDto.District;
                    seller.Address.Settlement = addressDto.Settlement;
                    seller.Address.Street = addressDto.Street;
                    seller.Address.HouseNumber = addressDto.HouseNumber;

                    var coords = await CoordinateHelper.GetCoordinates(seller.Address);
                    seller.Address.Latitude = coords.Latitude;
                    seller.Address.Longitude = coords.Longitude;

                    await DbContext.SaveChangesAsync();
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

        private async Task UpdateSellerImages(Seller seller, List<Microsoft.AspNetCore.Http.IFormFile> images)
        {
            if (seller.ImagesNames == null || images == null)
                seller.ImagesNames = new List<string>();

            if (images != null)
            {
                foreach (var imageName in seller.ImagesNames)
                {
                    if (!images.Any(file => file.FileName == imageName))
                    {
                        FileHelper.DeleteFile(imageName, Configuration["File:Images:Account"]);
                    }
                }

                foreach (var newImage in images)
                {
                    if (!seller.ImagesNames.Contains(newImage.FileName))
                    {
                        string imageName = await FileHelper.SaveFile(newImage, Configuration["File:Images:Account"]);
                        seller.ImagesNames.Add(imageName);
                    }
                }
            }
        }

        private async Task UpdateSellerSchedule(Seller seller, ScheduleDto dto)
        {
            if (seller.Schedule == null)
            {
                seller.Schedule = new Schedule() { Id = Guid.NewGuid() };
                DbContext.Schedules.Add(seller.Schedule);
                await DbContext.SaveChangesAsync();
            }

            await UpdateDayOfWeek(seller.Schedule.Monday, dto.Monday);
            await UpdateDayOfWeek(seller.Schedule.Tuesday, dto.Tuesday);
            await UpdateDayOfWeek(seller.Schedule.Wednesday, dto.Wednesday);
            await UpdateDayOfWeek(seller.Schedule.Thursday, dto.Thursday);
            await UpdateDayOfWeek(seller.Schedule.Friday, dto.Friday);
            await UpdateDayOfWeek(seller.Schedule.Sunday, dto.Sunday);
            await UpdateDayOfWeek(seller.Schedule.Saturday, dto.Saturday);
        }

        private async Task UpdateDayOfWeek(DayOfWeek dayOfWeek, DayOfWeekDto dto)
        {
            dayOfWeek.IsOpened = dto.IsOpened;
            dayOfWeek.StartHour = dto.StartHour;
            dayOfWeek.StartMinute = dto.StartMinute;
            dayOfWeek.EndHour = dto.EndHour;
            dayOfWeek.EndMinute = dto.EndMinute;
        }

        public async Task UpdateSellerCategoriesAndSubcategories(SellerCategoriesAndSubcategoriesDto dto, Guid accountId)
        {
            var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (seller == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            seller.Categories = dto.Categories ?? seller.Categories;
            seller.Subcategories = dto.Subcategories ?? seller.Subcategories;

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateFarmerPaymentData(ProducerPaymentDataDto dto, Guid accountId)
        {
            var farmer = await DbContext.Farmers.Include(f => f.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (farmer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            if (farmer.PaymentData == null)
            {
                farmer.PaymentData = new ProducerPaymentData() { Id = Guid.NewGuid() };
                DbContext.ProducerPaymentData.Add(farmer.PaymentData);
            }

            if (dto.HasOnlinePayment)
            {
                farmer.PaymentTypes = new List<PaymentType> { PaymentType.Online, PaymentType.Cash };
            }
            else
            {
                farmer.PaymentTypes = new List<PaymentType> { PaymentType.Cash };
            }

            farmer.PaymentData.CardNumber = dto.CardNumber;
            farmer.PaymentData.AccountNumber = dto.AccountNumber;
            farmer.PaymentData.BankUSREOU = dto.BankUSREOU;
            farmer.PaymentData.BIC = dto.BIC;
            farmer.PaymentData.CardExpirationYear = dto.CardExpirationYear;
            farmer.PaymentData.CardExpirationMonth = dto.CardExpirationMonth;

            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateSellerPaymentData(ProducerPaymentDataDto dto, Guid accountId)
        {
            var seller = await DbContext.Sellers.Include(s => s.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (seller == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            if (seller.PaymentData == null)
            {
                seller.PaymentData = new ProducerPaymentData { Id = Guid.NewGuid() };
                DbContext.ProducerPaymentData.Add(seller.PaymentData);
            }

            seller.PaymentData.CardNumber = dto.CardNumber;
            seller.PaymentData.AccountNumber = dto.AccountNumber;
            seller.PaymentData.BankUSREOU = dto.BankUSREOU;
            seller.PaymentData.BIC = dto.BIC;
            seller.PaymentData.CardExpirationYear = dto.CardExpirationYear;
            seller.PaymentData.CardExpirationMonth = dto.CardExpirationMonth;

            if (dto.HasOnlinePayment)
            {
                seller.PaymentTypes = new List<PaymentType> { PaymentType.Online, PaymentType.Cash };
            }
            else
            {
                seller.PaymentTypes = new List<PaymentType> { PaymentType.Cash };
            }

            await DbContext.SaveChangesAsync();
        }

        public async Task<CustomerOrderDetailsVm> GetCustomerOrderDetails(Guid accountId, ReceivingMethod receivingMethod)
        {
            var customer = await DbContext.Customers.Include(c => c.Address)
                .Include(c => c.PaymentData)
                .FirstOrDefaultAsync(a => a.Id == accountId);

            if (customer == null)
            {
                string message = $"Account with Id {accountId} was not found.";
                throw new NotFoundException(message, "AccountNotFound");
            }

            var vm = new CustomerOrderDetailsVm
            {
                Name = customer.Name,
                Surname = customer.Surname,
                Phone = customer.Phone,
                AdditionalPhone = customer.AdditionalPhone,
                PaymentData = Mapper.Map<CustomerPaymentDataVm>(customer.PaymentData)
            };

            if (receivingMethod == ReceivingMethod.Delivery)
            {
                vm.Address = Mapper.Map<CustomerAddressVm>(customer.Address);
            }

            return vm;
        }
    }

}
