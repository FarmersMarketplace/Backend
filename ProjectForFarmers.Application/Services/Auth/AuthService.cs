using FarmersMarketplace.Application.DataTransferObjects.Auth;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.ViewModels.Auth;
using FarmersMarketplace.Domain;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using FarmersMarketplace.Application.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using FarmersMarketplace.Application.ViewModels;

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
            var existingAccountWithSameEmail = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (existingAccountWithSameEmail != null) 
            {
                string message = $"Email {email} is already associated with another account.";
                string userFacingMessage = CultureHelper.Exception("DuplicateEmail", email);

                throw new DuplicateException(message, userFacingMessage);
            } 

            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null) 
            {
                string message = $"Account with Id {accountId} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountNotFound");

                throw new NotFoundException(message, userFacingMessage);
            } 
            
            account.Email = email;
            await DbContext.SaveChangesAsync();
        }

        public async Task<LoginVm> Login(LoginDto loginDto)
        {
            var account = DbContext.Accounts.FirstOrDefault(a => a.Email ==  loginDto.Email);
            if (account == null) 
            {
                string message = $"Account with email {loginDto.Email} was not found.";
                string userFacingMessage = CultureHelper.Exception("AccountWithEmailNotFound", loginDto.Email);

                throw new NotFoundException(message, userFacingMessage);
            } 
            else if(CryptoHelper.ComputeSha256Hash(loginDto.Password) != account.Password) throw new UnauthorizedAccessException("Invalid password.");

            var token = await JwtService.Authenticate(account);
            var vm = new LoginVm(token, account.Role, account.Id);

            return vm;
        }

        private async Task CreateAccount(Guid id, RegisterDto accountDto)
        {
            if(accountDto.Password != accountDto.ConfirmPassword) throw new ValidationException("The password and confirm password do not match.");
            var existingAccountWithSameEmail = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == accountDto.Email);
            if (existingAccountWithSameEmail != null) 
            {
                string message = $"Email {accountDto.Email} is already associated with another account.";
                string userFacingMessage = CultureHelper.Exception("EmailIsAssociatedWithAnotherAccount", accountDto.Email);

                throw new DuplicateException(message, userFacingMessage);
            } 

            var account = new Account
            {
                Id = id,
                Name = accountDto.Name,
                Surname = accountDto.Surname,
                Password = CryptoHelper.ComputeSha256Hash(accountDto.Password),
                Role = accountDto.Role
            };

            await DbContext.Accounts.AddAsync(account);
        }

        public async Task ResetPassword(Guid accountId, string email, ResetPasswordDto resetPasswordDto)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.Email == email);
            if (account == null)
            {
                string message = $"Account with Id {accountId} and email {email} was not found or email does not match the provided account.";
                string userFacingMessage = CultureHelper.Exception("AccountWithIdEmailNotFound");

                throw new NotFoundException(message, userFacingMessage);
            }
            
            else if(resetPasswordDto.Password != resetPasswordDto.ConfirmPassword) throw new UnauthorizedAccessException("Password and confirm password do not match.");

            account.Password = CryptoHelper.ComputeSha256Hash(resetPasswordDto.Password);
            await DbContext.SaveChangesAsync();
        }

        public async Task ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == forgotPasswordDto.Email);
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

            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == payload.Email);
            if (account == null) 
            {
                throw new Exception("Set role!");
                account = new Account 
                {
                    Id = Guid.NewGuid(),
                    Name = payload.GivenName,
                    Surname = payload.FamilyName,
                    Email = payload.Email,
                    //Role = Role.Customer
                };
                DbContext.Accounts.Add(account);
                await DbContext.SaveChangesAsync();
            }

            var token = await JwtService.Authenticate(account);
            var vm = new LoginVm(token, account.Role, account.Id);

            return vm;
        }
    }
}
