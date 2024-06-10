using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Auth;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Auth;
using FarmersMarketplace.Domain.Accounts;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using InvalidDataException = FarmersMarketplace.Application.Exceptions.InvalidDataException;

namespace FarmersMarketplace.Application.Services.Auth
{
    public class AuthService : Service, IAuthService
    {
        private readonly EmailHelper EmailHelper;
        private readonly JwtService JwtService;

        public AuthService(IMapper mapper, IApplicationDbContext dbContext, IConfiguration configuration) : base(mapper, dbContext, configuration)
        {
            JwtService = new JwtService(configuration);
            EmailHelper = new EmailHelper(configuration);
        }

        public async Task Register(RegisterDto dto)
        {
            Guid id = Guid.NewGuid();

            await CreateAccount(id, dto);
            string message = EmailContentBuilder.ConfirmationMessageBody(dto.Name, dto.Surname, dto.Email, await JwtService.EmailConfirmationToken(id, dto.Email));
            await EmailHelper.SendEmail(message, dto.Email, "Farmers marketplace Registration Confirmation");
            await DbContext.SaveChangesAsync();
        }

        public async Task ConfirmEmail(Guid accountId, string email)
        {
            if (await ExistsAccountWithEmail(email))
            {
                string message = $"Email {email} is already associated with another account.";
                throw new DuplicateException(message, "DuplicateEmail", email);
            }

            var account = await GetAccount(accountId);
            account.Email = email;


            await DbContext.SaveChangesAsync();
        }

        public async Task<Account> GetAccount(Guid accountId)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Id == accountId);

            if (customer != null)
            {
                return customer;
            }
            else
            {
                var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Id == accountId);

                if (seller != null)
                {
                    return seller;
                }
                else
                {
                    var farmer = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Id == accountId);

                    if (farmer != null)
                    {
                        return farmer;
                    }
                    else
                    {
                        string message = $"Account with Id {accountId} was not found.";
                        throw new NotFoundException(message, "AccountNotFound");
                    }
                }
            }
        }

        public async Task<Account> GetAccount(string email)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == email);

            if (customer != null)
            {
                return customer;
            }
            else
            {
                var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Email == email);

                if (seller != null)
                {
                    return seller;
                }
                else
                {
                    var farmer = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Email == email);

                    if (farmer != null)
                    {
                        return farmer;
                    }
                    else
                    {
                        string message = $"Account with email {email} was not found.";
                        throw new NotFoundException(message, "AccountWithEmailNotFound");
                    }
                }
            }
        }

        public async Task<LoginVm> Login(LoginDto dto)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == dto.Email);

            if (customer != null)
            {
                if (CryptoHelper.ComputeSha256Hash(dto.Password) != customer.Password)
                {
                    string message = $"Invalid password.";
                    throw new AuthorizationException(message, "InvalidPassword", dto.Email);
                }

                var token = await JwtService.Authenticate(customer.Id, Role.Customer);
                var vm = new LoginVm(token, Role.Customer, customer.Id);

                return vm;
            }
            else
            {
                var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Email == dto.Email);

                if (seller != null)
                {
                    if (CryptoHelper.ComputeSha256Hash(dto.Password) != seller.Password)
                    {
                        string message = $"Invalid password.";
                        throw new AuthorizationException(message, "InvalidPassword", dto.Email);
                    }

                    var token = await JwtService.Authenticate(seller.Id, Role.Seller);
                    var vm = new LoginVm(token, Role.Seller, seller.Id);

                    return vm;
                }
                else
                {
                    var farmer = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Email == dto.Email);

                    if (farmer != null)
                    {
                        if (CryptoHelper.ComputeSha256Hash(dto.Password) != farmer.Password)
                        {
                            string message = $"Invalid password.";
                            throw new AuthorizationException(message, "InvalidPassword", dto.Email);
                        }

                        var token = await JwtService.Authenticate(farmer.Id, Role.Farmer);
                        var vm = new LoginVm(token, Role.Farmer, farmer.Id);

                        return vm;
                    }
                    else
                    {
                        string message = $"Account with email {dto.Email} was not found.";
                        throw new NotFoundException(message, "AccountWithEmailNotFound", dto.Email);
                    }
                }
            }
        }

        private async Task CreateAccount(Guid id, RegisterDto dto)
        {
            if (dto.Password != dto.ConfirmPassword)
            {
                
                string message = $"The password and confirm password do not match.";
                throw new InvalidDataException(message, "PasswordNotMatchToConfirmPassword");
            }

            if (dto.Role == Role.Customer)
            {
                var existingCustomerWithSameEmail = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == dto.Email);

                if (existingCustomerWithSameEmail != null)
                {
                    string message = $"Email {dto.Email} is already associated with another account.";
                    throw new DuplicateException(message, "EmailIsAssociatedWithAnotherAccount", dto.Email);
                }

                var account = new Customer
                {
                    Id = id,
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Password = CryptoHelper.ComputeSha256Hash(dto.Password),
                };

                DbContext.Customers.Add(account);
            }
            else if (dto.Role == Role.Seller)
            {
                var existingSellerWithSameEmail = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Email == dto.Email);

                if (existingSellerWithSameEmail != null)
                {
                    string message = $"Email {dto.Email} is already associated with another account.";
                    throw new DuplicateException(message, "EmailIsAssociatedWithAnotherAccount", dto.Email);
                }

                var account = new Seller
                {
                    Id = id,
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Password = CryptoHelper.ComputeSha256Hash(dto.Password),
                };

                DbContext.Sellers.Add(account);
            }
            else if (dto.Role == Role.Farmer)
            {
                var existingFarmerWithSameEmail = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Email == dto.Email);

                if (existingFarmerWithSameEmail != null)
                {
                    string message = $"Email {dto.Email} is already associated with another account.";
                    throw new DuplicateException(message, "EmailIsAssociatedWithAnotherAccount", dto.Email);
                }

                var account = new Farmer
                {
                    Id = id,
                    Name = dto.Name,
                    Surname = dto.Surname,
                    Password = CryptoHelper.ComputeSha256Hash(dto.Password),
                };

                DbContext.Farmers.Add(account);
            }
            else
            {
                string message = $"Role is incorrect.";
                throw new InvalidDataException(message, "IncorrectRole", dto.Email);
            }
        }

        public async Task<bool> ExistsAccountWithEmail(string email)
        {
            var existingCustomerWithSameEmail = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == email);
            var existingFarmerWithSameEmail = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Email == email);
            var existingSellerWithSameEmail = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Email == email);

            return existingCustomerWithSameEmail != null
                || existingSellerWithSameEmail != null
                || existingFarmerWithSameEmail != null;
        }

        public async Task ResetPassword(Guid accountId, string email, ResetPasswordDto dto)
        {
            var account = await GetAccount(accountId);

            if (account == null || account.Email != email)
            {
                string message = $"Account with Id {accountId} and email {email} was not found or email does not match the provided account.";
                throw new NotFoundException(message, "AccountWithIdEmailNotFound");
            }

            else if (dto.Password != dto.ConfirmPassword)
            {
                string message = $"The password and confirm password do not match.";
                throw new InvalidDataException(message, "PasswordNotMatchToConfirmPassword");
            }

            account.Password = CryptoHelper.ComputeSha256Hash(dto.Password);
            await DbContext.SaveChangesAsync();
        }

        public async Task ForgotPassword(ForgotPasswordDto dto)
        {
            var account = await GetAccount(dto.Email);

            if (account == null)
            {
                string message = $"Account with email {dto.Email} was not found.";
                throw new NotFoundException(message, "AccountWithEmailNotFound", dto.Email);
            }
            string letterMessage = EmailContentBuilder.ResetPasswordMessageBody(account.Name, account.Surname, await JwtService.EmailConfirmationToken(account.Id, account.Email));
            await EmailHelper.SendEmail(letterMessage, dto.Email, "Password reset request for Agroforum account");
        }

        public async Task<LoginVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto dto)
        {
            throw new NotImplementedException("Set role");
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(dto.GoogleIdToken);
        }
    }
}
