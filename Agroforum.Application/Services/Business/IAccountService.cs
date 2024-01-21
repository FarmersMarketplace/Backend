using Agroforum.Application.DataTransferObjects.Account;
using Agroforum.Application.ViewModels;
using Agroforum.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Business
{
    public interface IAccountService
    {
        Task Update(UpdateAccountDto updateAccountDto);
        Task UpdatePhoto(UpdateAccountPhotoDto updateAccountPhototDto);
        Task<AccountVm> Get(Guid accountId);
    }
}
