using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Exceptions;
using Agroforum.Application.Interfaces;
using Agroforum.Application.ViewModels.Auth;
using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private IAgroforumDbContext DbContext { get; set; }
        private EmailConfirmationService EmailService { get; set; }

        public AuthService(IAgroforumDbContext dbContext, IConfiguration configuration)
        {
            DbContext = dbContext;
            EmailService = new EmailConfirmationService(configuration);
        }

        public async Task<RegisterVm> Register(RegisterDto accountDto)
        {
            Guid id = Guid.NewGuid();

            await CreateAccount(id, accountDto);
            await EmailService.SendEmailAsync(id, accountDto.Email);

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

        public async Task<TokenVm> Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        private async Task CreateAccount(Guid id, RegisterDto accountDto)
        {
            var account = new Account
            {
                Id = id,
                Name = accountDto.Name,
                Surname = accountDto.Surname,
                Email = accountDto.Email,
            };
            if (accountDto.IsFarmer) account.Roles.Add(Role.Farmer);

            await DbContext.Accounts.AddAsync(account);
        }

    }
}
