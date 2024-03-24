using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Application.ViewModels.Auth;
using FarmersMarketplace.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InvalidDataException = FarmersMarketplace.Application.Exceptions.InvalidDataException;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService AccountService;
        private Guid AccountId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(CustomerVm), 200)]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid accountId)
        {
            var vm = await AccountService.GetCustomer(accountId);
            return Ok(vm);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateCustomer([FromForm] UpdateCustomerDto dto)
        {
            await AccountService.UpdateCustomer(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateCustomerPaymentData([FromBody] CustomerPaymentDataDto dto)
        {
            await AccountService.UpdateCustomerPaymentData(dto, AccountId);
            return NoContent();
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(SellerVm), 200)]
        public async Task<IActionResult> GetSeller([FromRoute] Guid accountId)
        {
            var vm = await AccountService.GetSeller(accountId);
            return Ok(vm);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSeller([FromForm] UpdateSellerDto dto)
        {
            await AccountService.UpdateSeller(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSellerCategoriesAndSubcategories([FromBody] SellerCategoriesAndSubcategoriesDto dto)
        {
            await AccountService.UpdateSellerCategoriesAndSubcategories(dto, AccountId);
            return NoContent();
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(FarmerVm), 200)]
        public async Task<IActionResult> GetFarmer([FromRoute] Guid accountId)
        {
            var vm = await AccountService.GetFarmer(accountId);
            return Ok(vm);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateFarmer([FromForm] UpdateFarmerDto dto)
        {
            await AccountService.UpdateFarmer(dto, AccountId);
            return NoContent();
        }

        [HttpPut("{producer}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateProducerPaymentData([FromBody] ProducerPaymentDataDto dto, [FromRoute] Role producer)
        {
            await AccountService.UpdateProducerPaymentData(dto, AccountId, producer);
            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAccount()
        {
            var roleStr = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleStr) || !Enum.TryParse<Role>(roleStr, out var role))
            {
                string message = $"Failed to retrieve or parse role claim from JWT.";
                string userFacingMessage = CultureHelper.Exception("IncorrectRole");

                throw new InvalidDataException(message, userFacingMessage);
            }

            await AccountService.DeleteAccount(role, AccountId);
            return NoContent();
        }
    }


}
