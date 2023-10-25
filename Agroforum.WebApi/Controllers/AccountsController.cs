using Agroforum.WebApi.DataTransferObjects;
using Agroforum.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Agroforum.WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountsController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto accountDto)
        {


            return Ok(new RegisterVm());
        }

        [HttpPut]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationDto emailConfirmationDto)
        {
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> AddPhoneNumber([FromBody] AddPhoneDto addPhoneDto)
        {
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> ConfirmPhone([FromBody] PhoneConfirmationDto phoneConfirmationDto)
        {
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            return Ok(new TokenVm());
        }
    }
}
