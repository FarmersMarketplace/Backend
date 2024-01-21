using Agroforum.Application.DataTransferObjects.Account;
using Agroforum.Application.Exceptions;
using Agroforum.Application.Interfaces;
using Agroforum.Application.ViewModels;
using Agroforum.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Business
{
    public class AccountService : IAccountService
    {
        private readonly IAgroforumDbContext DbContext;

        public AccountService(IAgroforumDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task Update(UpdateAccountDto updateAccountDto)
        {
            var account = await DbContext.Accounts.FirstOrDefaultAsync(a => a.Id == updateAccountDto.Id);
            if (account == null) throw new NotFoundException($"Account with Id {updateAccountDto.Id} not found.");

            account.Name = updateAccountDto.Name;
            account.Surname = updateAccountDto.Surname;
            
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdatePhoto(UpdateAccountPhotoDto updateAccountPhototDto)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountVm> Get(Guid accountId)
        {
            throw new NotImplementedException("Photo");
        }
    }
}
