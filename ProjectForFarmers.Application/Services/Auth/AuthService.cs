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

        public async Task Register(RegisterDto accountDto)
        {
            Guid id = Guid.NewGuid();

            await CreateAccount(id, accountDto);
            string message = EmailContentBuilder.ConfirmationMessageBody(accountDto.Name, accountDto.Surname, accountDto.Email, await JwtService.EmailConfirmationToken(id, accountDto.Email));
            await EmailHelper.SendEmail(message, accountDto.Email, "Farmers marketplace Registration Confirmation");
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

        public async Task<LoginVm> Login(LoginDto loginDto)
        {
            var customer = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == loginDto.Email);

            if (customer != null)
            {
                if (CryptoHelper.ComputeSha256Hash(loginDto.Password) != customer.Password)
                {
                    string message = $"Invalid password.";
                    string userFacingMessage = CultureHelper.Exception("InvalidPassword", loginDto.Email);

                    throw new AuthorizationException(message, userFacingMessage);
                }

                var token = await JwtService.Authenticate(customer.Id, Role.Customer);
                var vm = new LoginVm(token, Role.Customer, customer.Id);

                return vm;
            }
            else
            {
                var seller = await DbContext.Sellers.FirstOrDefaultAsync(a => a.Email == loginDto.Email);

                if (seller != null)
                {
                    if (CryptoHelper.ComputeSha256Hash(loginDto.Password) != seller.Password)
                    {
                        string message = $"Invalid password.";
                        string userFacingMessage = CultureHelper.Exception("InvalidPassword", loginDto.Email);

                        throw new AuthorizationException(message, userFacingMessage);
                    }

                    var token = await JwtService.Authenticate(seller.Id, Role.Seller);
                    var vm = new LoginVm(token, Role.Seller, seller.Id);

                    return vm;
                }
                else
                {
                    var farmer = await DbContext.Farmers.FirstOrDefaultAsync(a => a.Email == loginDto.Email);

                    if (farmer != null)
                    {
                        if (CryptoHelper.ComputeSha256Hash(loginDto.Password) != farmer.Password)
                        {
                            string message = $"Invalid password.";
                            string userFacingMessage = CultureHelper.Exception("InvalidPassword", loginDto.Email);

                            throw new AuthorizationException(message, userFacingMessage);
                        }

                        var token = await JwtService.Authenticate(farmer.Id, Role.Farmer);
                        var vm = new LoginVm(token, Role.Farmer, farmer.Id);

                        return vm;
                    }
                    else
                    {
                        string message = $"Account with email {loginDto.Email} was not found.";
                        string userFacingMessage = CultureHelper.Exception("AccountWithEmailNotFound", loginDto.Email);

                        throw new NotFoundException(message, userFacingMessage);
                    }
                }
            }
        }

        private async Task CreateAccount(Guid id, RegisterDto accountDto)
        {
            if(accountDto.Password != accountDto.ConfirmPassword)
            {
                string message = $"The password and confirm password do not match.";
                string userFacingMessage = CultureHelper.Exception("PasswordNotMatchToConfirmPassword");

                throw new InvalidDataException(message, userFacingMessage);
            }

            if (await ExistsAccountWithEmail(accountDto.Email)) 
            {
                string message = $"Email {accountDto.Email} is already associated with another account.";
                string userFacingMessage = CultureHelper.Exception("EmailIsAssociatedWithAnotherAccount", accountDto.Email);

                throw new DuplicateException(message, userFacingMessage);
            }

            if (accountDto.Role == Role.Customer) 
            {
                var account = new Customer
                {
                    Id = id,
                    Name = accountDto.Name,
                    Surname = accountDto.Surname,
                    Password = CryptoHelper.ComputeSha256Hash(accountDto.Password),
                };

                await DbContext.Customers.AddAsync(account);
            }
            else if (accountDto.Role == Role.Seller) 
            {
                var account = new Seller
                {
                    Id = id,
                    Name = accountDto.Name,
                    Surname = accountDto.Surname,
                    Password = CryptoHelper.ComputeSha256Hash(accountDto.Password),
                };

                await DbContext.Sellers.AddAsync(account);
            }
            else if (accountDto.Role == Role.Farmer)
            {
                var account = new Farmer
                {
                    Id = id,
                    Name = accountDto.Name,
                    Surname = accountDto.Surname,
                    Password = CryptoHelper.ComputeSha256Hash(accountDto.Password),
                };

                await DbContext.Farmers.AddAsync(account);
            }
            else 
            {
                string message = $"Role is incorrect.";
                string userFacingMessage = CultureHelper.Exception("IncorrectRole", accountDto.Email);

                throw new InvalidDataException(message, userFacingMessage);
            }
        }

        public async Task<bool> ExistsAccountWithEmail(string email) 
        {
            var existingCustomerWithSameEmail = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == email);
            var existingSellerWithSameEmail = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == email);
            var existingFarmerWithSameEmail = await DbContext.Customers.FirstOrDefaultAsync(a => a.Email == email);

            return existingCustomerWithSameEmail != null
                || existingSellerWithSameEmail != null
                || existingFarmerWithSameEmail != null;
        }

        public async Task ResetPassword(Guid accountId, string email, ResetPasswordDto resetPasswordDto)
        {
            var account = await GetAccount(accountId);

            if (account == null || account.Email != email)
            {
                string message = $"Account with Id {accountId} and email {email} was not found or email does not match the provided account.";
                string userFacingMessage = CultureHelper.Exception("AccountWithIdEmailNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }
            
            else if(resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
            {
                string message = $"The password and confirm password do not match.";
                string userFacingMessage = CultureHelper.Exception("PasswordNotMatchToConfirmPassword");

                throw new InvalidDataException(message, userFacingMessage);
            }

            account.Password = CryptoHelper.ComputeSha256Hash(resetPasswordDto.Password);
            await DbContext.SaveChangesAsync();
        }

        public async Task ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var account = await GetAccount(forgotPasswordDto.Email);

            if (account == null)
            {
                string message = $"Account with email {forgotPasswordDto.Email} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountWithEmailNotFound", forgotPasswordDto.Email);

                throw new NotFoundException(message, userFacingMessage);
            }
            string letterMessage = EmailContentBuilder.ResetPasswordMessageBody(account.Name, account.Surname, await JwtService.EmailConfirmationToken(account.Id, account.Email));
            await EmailHelper.SendEmail(letterMessage, forgotPasswordDto.Email, "Password reset request for Agroforum account");
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

        public async Task<LoginVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto authenticateWithGoogleDto)
        {
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(authenticateWithGoogleDto.GoogleIdToken);

            throw new Exception("Set role!");
        }
    }
}
