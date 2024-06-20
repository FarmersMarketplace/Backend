using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Farm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FarmController : ControllerBase
    {
        private readonly IFarmService FarmService;
        private Guid AccountId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public FarmController(IFarmService farmService)
        {
            FarmService = farmService;
        }

        [HttpGet("{farmId}")]
        [ProducesResponseType(typeof(FarmForProducerVm), 200)]
        public async Task<IActionResult> GetForProducer([FromRoute] Guid farmId)
        {
            var request = await FarmService.GetForProducer(farmId);
            return Ok(request);
        }

        [HttpGet]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(typeof(AccountNumberDataVm), 200)]
        public async Task<IActionResult> CopyOwnerAccountNumberData()
        {
            var vm = await FarmService.CopyOwnerAccountNumberData(AccountId);
            return Ok(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(typeof(CardDataVm), 200)]
        public async Task<IActionResult> CopyOwnerCardData()
        {
            var vm = await FarmService.CopyOwnerCardData(AccountId);
            return Ok(vm);
        }

        [HttpGet]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(typeof(FarmListVm), 200)]
        public async Task<IActionResult> GetAllForProducer()
        {
            var vm = await FarmService.GetAllForProducer(AccountId);
            return Ok(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromForm] CreateFarmDto dto)
        {
            dto.OwnerId = AccountId;
            await FarmService.Create(dto);
            return NoContent();
        }

        [HttpDelete("{farmId}")]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromRoute] Guid farmId)
        {
            await FarmService.Delete(farmId, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Farmer")]
        public async Task<IActionResult> Update([FromForm] UpdateFarmDto dto)
        {
            await FarmService.Update(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateFarmCategoriesAndSubcategories([FromBody] UpdateFarmCategoriesAndSubcategoriesDto dto)
        {
            await FarmService.UpdateFarmCategoriesAndSubcategories(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Farmer")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdatePaymentData([FromBody] FarmPaymentDataDto dto)
        {
            await FarmService.UpdatePaymentData(dto, AccountId);
            return NoContent();
        }
    }
}
