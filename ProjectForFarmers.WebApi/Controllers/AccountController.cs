using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public async Task<IActionResult> GetCustomer([FromRoute] Guid accountId)
        {
            var customer = await AccountService.GetCustomer(accountId);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerDto dto)
        {
            await AccountService.UpdateCustomer(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerPaymentData([FromBody] CustomerPaymentDataDto dto)
        {
            await AccountService.UpdateCustomerPaymentData(dto, AccountId);
            return NoContent();
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetSeller([FromRoute] Guid accountId)
        {
            var seller = await AccountService.GetSeller(accountId);
            return Ok(seller);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSeller([FromBody] UpdateSellerDto dto)
        {
            await AccountService.UpdateSeller(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSellerCategoriesAndSubcategories([FromBody] SellerCategoriesAndSubcategoriesDto dto)
        {
            await AccountService.UpdateSellerCategoriesAndSubcategories(dto, AccountId);
            return NoContent();
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetFarmer([FromRoute] Guid accountId)
        {
            var farmer = await AccountService.GetFarmer(accountId);
            return Ok(farmer);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFarmer([FromBody] UpdateFarmerDto dto)
        {
            await AccountService.UpdateFarmer(dto, AccountId);
            return NoContent();
        }

        [HttpPut("{producer}")]
        public async Task<IActionResult> UpdateProducerPaymentData([FromBody] ProducerPaymentDataDto dto, [FromRoute] Role producer)
        {
            await AccountService.UpdateProducerPaymentData(dto, AccountId, producer);
            return NoContent();
        }

        [HttpDelete("{role}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] Role role)
        {
            await AccountService.DeleteAccount(role, AccountId);
            return NoContent();
        }
    }



}
