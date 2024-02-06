using ProjectForFarmers.Application.DataTransferObjects.Account;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Application.Services.Business;
using ProjectForFarmers.Application.ViewModels;
using ProjectForFarmers.Application.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ProjectForFarmers.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService AccountService;

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update([FromBody] UpdateAccountDto updateAccountDto)
        {
            //await AccountService.Update(updateAccountDto);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdatePhoto([FromBody] UpdateAccountPhotoDto updateAccountPhotoDto)
        {
            //await AccountService.UpdatePhoto(updateAccountPhotoDto);
            return NoContent();
        }

        //[HttpGet("{accountId}")]
        //[ProducesResponseType(typeof(AccountVm), 200)]
        //public async Task<IActionResult> Get(Guid accountId)
        //{
        //    var accountVm = await AccountService.Get(accountId);
        //    return Ok(accountVm);
        //}
    }
}
