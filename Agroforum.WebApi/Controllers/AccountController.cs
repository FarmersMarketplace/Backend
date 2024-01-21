using Agroforum.Application.DataTransferObjects.Account;
using Agroforum.Application.Interfaces;
using Agroforum.Application.Services.Business;
using Agroforum.Application.ViewModels;
using Agroforum.Application.ViewModels.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Agroforum.WebApi.Controllers
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
            await AccountService.Update(updateAccountDto);
            return NoContent();
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdatePhoto([FromBody] UpdateAccountPhotoDto updateAccountPhotoDto)
        {
            await AccountService.UpdatePhoto(updateAccountPhotoDto);
            return NoContent();
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(AccountVm), 200)]
        public async Task<IActionResult> Get(Guid accountId)
        {
            var accountVm = await AccountService.Get(accountId);
            return Ok(accountVm);
        }
    }
}
