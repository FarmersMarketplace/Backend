using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using InvalidDataException = FarmersMarketplace.Application.Exceptions.InvalidDataException;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateCustomer([FromForm] UpdateCustomerDto dto)
        {
            await AccountService.UpdateCustomer(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Customer")]
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
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> UpdateSeller([FromForm] UpdateSellerDto dto)
        {
            await AccountService.UpdateSeller(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Seller")]
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
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateFarmer([FromForm] UpdateFarmerDto dto)
        {
            await AccountService.UpdateFarmer(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateFarmerPaymentData([FromBody] ProducerPaymentDataDto dto)
        {
            await AccountService.UpdateFarmerPaymentData(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSellerPaymentData([FromBody] ProducerPaymentDataDto dto)
        {
            await AccountService.UpdateSellerPaymentData(dto, AccountId);
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(204)]
        public async Task<IActionResult> DeleteAccount()
        {
            var roleStr = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleStr) || !Enum.TryParse<Role>(roleStr, out var role))
            {
                string message = $"Failed to retrieve or parse role claim from JWT.";
                throw new InvalidDataException(message, "IncorrectRole");
            }

            await AccountService.DeleteAccount(role, AccountId);
            return NoContent();
        }

        [HttpGet("{receivingMethod}")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(typeof(CustomerOrderDetailsVm), 200)]
        public async Task<IActionResult> GetCustomerOrderDetails(ReceivingMethod receivingMethod)
        {
            var vm = await AccountService.GetCustomerOrderDetails(AccountId, receivingMethod);
            return NoContent();
        }
    }


}
