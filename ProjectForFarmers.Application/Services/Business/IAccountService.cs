using ProjectForFarmers.Application.DataTransferObjects;
using ProjectForFarmers.Application.DataTransferObjects.Account;
using ProjectForFarmers.Application.DataTransferObjects.Farm;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IAccountService
    {
        public Task Update(UpdateAccountDto updateAccountDto);
    }
}
