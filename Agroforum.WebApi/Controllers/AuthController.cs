using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Services;
using Agroforum.Application.ViewModels;
using Agroforum.Application.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agroforum.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService AuthService;

        public AuthController(IAuthService authService)
        {
            AuthService = authService;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Register([FromBody] RegisterDto accountDto)
        {
            await AuthService.Register(accountDto);
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ConfirmEmail()
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            await AuthService.ConfirmEmail(accountId, email);
            return NoContent();
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(JwtVm), 200)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var response = await AuthService.Login(loginDto);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(JwtVm), 200)]
        public async Task<IActionResult> AuthenticateWithGoogle([FromBody] AuthenticateWithGoogleDto authenticateWithGoogleDto)
        {
            var response = await AuthService.AuthenticateWithGoogle(authenticateWithGoogleDto);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            await AuthService.ForgotPassword(forgotPasswordDto);
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            await AuthService.ResetPassword(accountId, email, resetPasswordDto);

            return NoContent();
        }
    }
}
