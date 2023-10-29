using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Services;
using Agroforum.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agroforum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private IAuthService AuthService;

        public AuthController(IAuthService authService)
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
        [Authorize]
        public async Task<IActionResult> ConfirmEmail()
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            await AuthService.ConfirmEmail(accountId, email);

            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await AuthService.Login(loginDto);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            await AuthService.ForgotPassword(forgotPasswordDto);

            return NoContent();
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            await AuthService.ResetPassword(accountId, email, resetPasswordDto);

            return NoContent();
        }

    }
}
