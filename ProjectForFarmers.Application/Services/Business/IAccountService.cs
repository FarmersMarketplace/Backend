using ProjectForFarmers.Application.DataTransferObjects.Account;
using ProjectForFarmers.Application.ViewModels;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IAccountService
    {
        Task Update(UpdateAccountDto updateAccountDto);
        Task UpdatePhoto(UpdateAccountPhotoDto updateAccountPhototDto);
        Task<AccountVm> Get(Guid accountId);
    }
}
