using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Auth;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Auth;
using FarmersMarketplace.Domain;
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
                string userFacingMessage = CultureHelper.Exception("DuplicateEmail", email);

                throw new DuplicateException(message, userFacingMessage);
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
                        string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                        throw new NotFoundException(message, userFacingMessage);
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
                        string userFacingMessage = CultureHelper.Exception("AccountWithEmailNotFound");

                        throw new NotFoundException(message, userFacingMessage);
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
                    string userFacingMessage = CultureHelper.Exception("InvalidPassword", dto.Email);

                    throw new AuthorizationException(message, userFacingMessage);
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
                        string userFacingMessage = CultureHelper.Exception("InvalidPassword", dto.Email);

                        throw new AuthorizationException(message, userFacingMessage);
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
                            string userFacingMessage = CultureHelper.Exception("InvalidPassword", dto.Email);

                            throw new AuthorizationException(message, userFacingMessage);
                        }

                        var token = await JwtService.Authenticate(farmer.Id, Role.Farmer);
                        var vm = new LoginVm(token, Role.Farmer, farmer.Id);

                        return vm;
                    }
                    else
                    {
                        string message = $"Account with email {dto.Email} was not found.";
                        string userFacingMessage = CultureHelper.Exception("AccountWithEmailNotFound", dto.Email);

                        throw new NotFoundException(message, userFacingMessage);
                    }
                }
            }
        }

        private async Task CreateAccount(Guid id, RegisterDto dto)
        {
            if(dto.Password != dto.ConfirmPassword)
            {
                string message = $"The password and confirm password do not match.";
                string userFacingMessage = CultureHelper.Exception("PasswordNotMatchToConfirmPassword");

                throw new InvalidDataException(message, userFacingMessage);
            }

            if (dto.Role == Role.Customer) 
            {
                var existingCustomerWithSameEmail = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == dto.Email);

                if (existingCustomerWithSameEmail != null)
                {
                    string message = $"Email {dto.Email} is already associated with another account.";
                    string userFacingMessage = CultureHelper.Exception("EmailIsAssociatedWithAnotherAccount", dto.Email);

                    throw new DuplicateException(message, userFacingMessage);
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
                    string userFacingMessage = CultureHelper.Exception("EmailIsAssociatedWithAnotherAccount", dto.Email);

                    throw new DuplicateException(message, userFacingMessage);
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
                    string userFacingMessage = CultureHelper.Exception("EmailIsAssociatedWithAnotherAccount", dto.Email);

                    throw new DuplicateException(message, userFacingMessage);
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
                string userFacingMessage = CultureHelper.Exception("IncorrectRole", dto.Email);

                throw new InvalidDataException(message, userFacingMessage);
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
                string userFacingMessage = CultureHelper.Exception("AccountWithIdEmailNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }
            
            else if(dto.Password != dto.ConfirmPassword)
            {
                string message = $"The password and confirm password do not match.";
                string userFacingMessage = CultureHelper.Exception("PasswordNotMatchToConfirmPassword");

                throw new InvalidDataException(message, userFacingMessage);
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
                string userFacingMessage = CultureHelper.Exception("AccountWithEmailNotFound", dto.Email);

                throw new NotFoundException(message, userFacingMessage);
            }
            string letterMessage = EmailContentBuilder.ResetPasswordMessageBody(account.Name, account.Surname, await JwtService.EmailConfirmationToken(account.Id, account.Email));
            await EmailHelper.SendEmail(letterMessage, dto.Email, "Password reset request for Agroforum account");
        }

        public async Task ConfirmFarmEmail(Guid farmId, string email)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(a => a.Id == farmId);

            if (farm == null)
            {
                string message = $"Farm with Id {farmId} was not found.";
                string userFacingMessage = CultureHelper.Exception("FarmNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }

            farm.ContactEmail = email;
            await DbContext.SaveChangesAsync();
        }

        public async Task<LoginVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto dto)
        {
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(dto.GoogleIdToken);

            throw new Exception("Set role!");
        }
    }
}
