using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Services;
using Agroforum.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Agroforum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountsController : ControllerBase
    {
        private IAuthService AuthService;

        public AccountsController(IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto accountDto)
        {
            var response = await AuthService.Register(accountDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDto emailConfirmationDto)
        {
            await AuthService.ConfirmEmail(emailConfirmationDto);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> AddPhoneNumber([FromBody] AddPhoneDto addPhoneDto)
        {
            await AuthService.AddPhoneNumber(addPhoneDto);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ConfirmPhone([FromBody] PhoneConfirmationDto phoneConfirmationDto)
        {
            await AuthService.ConfirmPhone(phoneConfirmationDto);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await AuthService.Login(loginDto);
            return Ok(response);
        }
    }
}
