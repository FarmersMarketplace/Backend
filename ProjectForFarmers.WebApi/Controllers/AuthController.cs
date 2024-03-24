using FarmersMarketplace.Application.DataTransferObjects.Auth;
using FarmersMarketplace.Application.Services;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Application.ViewModels.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmersMarketplace.WebApi.Controllers
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
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            await AuthService.Register(dto);
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
        
        [HttpGet]
        [ProducesResponseType(typeof(LoginVm), 200)]
        public async Task<IActionResult> Login([FromQuery] LoginDto dto)
        {
            var response = await AuthService.Login(dto);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(LoginVm), 200)]
        public async Task<IActionResult> AuthenticateWithGoogle([FromBody] AuthenticateWithGoogleDto dto)
        {
            var response = await AuthService.AuthenticateWithGoogle(dto);
            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            await AuthService.ForgotPassword(dto);
            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var accountId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            await AuthService.ResetPassword(accountId, email, dto);

            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ConfirmFarmEmail()
        {
            var farmId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            await AuthService.ConfirmFarmEmail(farmId, email);
            return NoContent();
        }
    }
}
