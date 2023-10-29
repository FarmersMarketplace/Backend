using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Exceptions;
using Agroforum.Application.Interfaces;
using Agroforum.Application.ViewModels.Auth;
using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;


namespace Agroforum.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private IAgroforumDbContext DbContext { get; set; }
        private EmailConfirmationService EmailService { get; set; }
        private JwtService JwtService { get; set; }

        public AuthService(IAgroforumDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            EmailService = new EmailConfirmationService(configuration);
            JwtService = new JwtService(configuration);
        }

        public async Task<RegisterVm> Register(RegisterDto accountDto)
        {
            Guid id = Guid.NewGuid();

            await CreateAccount(id, accountDto);
            await EmailService.SendConfirmationEmail(await JwtService.EmailConfirmationToken(id, accountDto.Email), accountDto.Email);

            await DbContext.SaveChangesAsync();
            return new RegisterVm(id);
        }

        public async Task ConfirmEmail(Guid accountId, string email)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
            if (account == null) throw new NotFoundException($"Account with Id {accountId} not found in the database.");

            account.Email = email;
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

            var account = new Account
            {
                Id = id,
                Name = accountDto.Name,
                Surname = accountDto.Surname,
                Email = accountDto.Email,
                Password = accountDto.Password,
                Roles = new List<Role>()
            };
            if (accountDto.IsFarmer) account.Roles.Add(Role.Farmer);

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

            await EmailService.SendPasswordResetEmail(await JwtService.EmailConfirmationToken(account.Id, account.Email), forgotPasswordDto.Email);
        }
    }
}
