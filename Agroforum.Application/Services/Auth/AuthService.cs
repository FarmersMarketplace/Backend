using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Exceptions;
using Agroforum.Application.Interfaces;
using Agroforum.Application.ViewModels.Auth;
using Agroforum.Domain;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Agroforum.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private IAgroforumDbContext DbContext { get; set; }
        private EmailService EmailService { get; set; }
        private JwtService JwtService { get; set; }

        public AuthService(IAgroforumDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            EmailService = new EmailService(configuration);
            JwtService = new JwtService(configuration);
        }

        public async Task Register(RegisterDto accountDto)
        {
            Guid id = Guid.NewGuid();

            await CreateAccount(id, accountDto);
            string message = EmailContentBuilder.ConfirmationMessageBody(accountDto.Name, accountDto.Surname, accountDto.Email, await JwtService.EmailConfirmationToken(id, accountDto.Email));
            await EmailService.SendEmail(message, accountDto.Email, "[ServiceName] Registration Confirmation");
            await DbContext.SaveChangesAsync();
        }

        public async Task ConfirmEmail(Guid accountId, string email)
        {
            var existingAccountWithSameEmail = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            if (existingAccountWithSameEmail != null) throw new DuplicateEmailException($"Email '{email}' is already associated with another account.");

            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);

            if (account == null) throw new NotFoundException($"Account with Id {accountId} not found in the database.");
            
            account.Email = email;
            account.Roles.Add(Role.Customer);
            await DbContext.SaveChangesAsync();
        }

        public async Task<JwtVm> Login(LoginDto loginDto)
        {
            var account = DbContext.Accounts.FirstOrDefault(a => a.Email ==  loginDto.Email);
            if (account == null) throw new NotFoundException($"Account with {loginDto.Email} email is not found.");
            else if(loginDto.Password != account.Password) throw new UnauthorizedAccessException("Invalid password.");

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
                Password = accountDto.Password,
                Roles = new List<Role>()
            };
            account.Roles.Add(accountDto.Role);

            await DbContext.Accounts.AddAsync(account);
        }

        public async Task ResetPassword(Guid accountId, string email, ResetPasswordDto resetPasswordDto)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId && a.Email == email);
            if (account == null) throw new NotFoundException("Account not found or email does not match the provided account.");
            else if(resetPasswordDto.Password != resetPasswordDto.ConfirmPassword) throw new UnauthorizedAccessException("Password and confirm password do not match.");

            account.Password = resetPasswordDto.Password;
            await DbContext.SaveChangesAsync();
        }

        public async Task ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == forgotPasswordDto.Email);
            if (account == null) throw new NotFoundException($"Account with email {forgotPasswordDto.Email} is not found.");
            string message = EmailContentBuilder.ResetPasswordMessageBody(account.Name, account.Surname, await JwtService.EmailConfirmationToken(account.Id, account.Email));
            await EmailService.SendEmail(message, forgotPasswordDto.Email, "Password reset request for Agroforum account");
        }

        public async Task<JwtVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto authenticateWithGoogleDto)
        {
            Payload payload = await GoogleJsonWebSignature.ValidateAsync(authenticateWithGoogleDto.GoogleIdToken);

            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Email == payload.Email);
            if (account == null) 
            {
                account = new Account 
                {
                    Id = Guid.NewGuid(),
                    Name = payload.GivenName,
                    Surname = payload.FamilyName,
                    Email = payload.Email,
                    Roles = new List<Role> { Role.Customer}
                };
                DbContext.Accounts.Add(account);
                await DbContext.SaveChangesAsync();
            }

            var request = await JwtService.Authenticate(account);

            return request;
        }
    }
}
