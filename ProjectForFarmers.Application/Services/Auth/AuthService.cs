using ProjectForFarmers.Application.DataTransferObjects.Auth;
using ProjectForFarmers.Application.Exceptions;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.ViewModels.Auth;
using ProjectForFarmers.Domain;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using ProjectForFarmers.Application.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ProjectForFarmers.Application.ViewModels;

namespace ProjectForFarmers.Application.Services.Auth
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
            await EmailHelper.SendEmail(message, accountDto.Email, "[ServiceName] Registration Confirmation");
            await DbContext.SaveChangesAsync();
        }

        public async Task ConfirmEmail(Guid accountId, string email)
        {
            var existingAccountWithSameEmail = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (existingAccountWithSameEmail != null) throw new DuplicateEmailException($"Email '{email}' is already associated with another account.");

            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null) throw new NotFoundException($"Account with Id {accountId} not found.");
            
            account.Email = email;
            await DbContext.SaveChangesAsync();
        }

        public async Task<JwtVm> Login(LoginDto loginDto)
        {
            var account = DbContext.Accounts.FirstOrDefault(a => a.Email ==  loginDto.Email);
            if (account == null) throw new NotFoundException($"Account with {loginDto.Email} email is not found.");
            else if(CryptoHelper.ComputeSha256Hash(loginDto.Password) != account.Password) throw new UnauthorizedAccessException("Invalid password.");

            var request = await JwtService.Authenticate(account);

            return request;
        }

        private async Task CreateAccount(Guid id, RegisterDto accountDto)
        {
            if(accountDto.Password != accountDto.ConfirmPassword) throw new ValidationException("The password and confirm password do not match.");
            var existingAccountWithSameEmail = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == accountDto.Email);
            if (existingAccountWithSameEmail != null) throw new DuplicateEmailException($"Email '{accountDto.Email}' is already associated with another account.");

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
            if (account == null) throw new NotFoundException("Account not found or email does not match the provided account.");
            else if(resetPasswordDto.Password != resetPasswordDto.ConfirmPassword) throw new UnauthorizedAccessException("Password and confirm password do not match.");

            account.Password = CryptoHelper.ComputeSha256Hash(resetPasswordDto.Password);
            await DbContext.SaveChangesAsync();
        }

        public async Task ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == forgotPasswordDto.Email);
            if (account == null) throw new NotFoundException($"Account with email {forgotPasswordDto.Email} is not found.");
            string message = EmailContentBuilder.ResetPasswordMessageBody(account.Name, account.Surname, await JwtService.EmailConfirmationToken(account.Id, account.Email));
            await EmailHelper.SendEmail(message, forgotPasswordDto.Email, "Password reset request for Agroforum account");
        }

        public async Task ConfirmFarmEmail(Guid farmId, string email)
        {
            var farm = await DbContext.Farms.FirstOrDefaultAsync(a => a.Id == farmId);

            if (farm == null) throw new NotFoundException($"Farm with Id {farmId} not found.");

            farm.ContactEmail = email;
            await DbContext.SaveChangesAsync();
        }

        public async Task<JwtVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto authenticateWithGoogleDto)
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

            var request = await JwtService.Authenticate(account);

            return request;
        }
    }
}
