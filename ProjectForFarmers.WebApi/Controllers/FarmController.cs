using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using FarmersMarketplace.Application.ViewModels.Farm;

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
        [ProducesResponseType(typeof(FarmVm), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid farmId)
        {
            var request = await FarmService.Get(farmId);
            return Ok(request);
        }

        [HttpGet]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(typeof(FarmListVm), 200)]
        public async Task<IActionResult> GetAll()
        {
            var userId = AccountId;
            var vm = await FarmService.GetAll(userId);
            return Ok(vm);
        }

        [HttpPost]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromForm] CreateFarmDto dto)
        {
            dto.OwnerId = AccountId;
            await FarmService.Create(dto);
            return NoContent();
        }

        [HttpDelete("{farmId}")]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromRoute] Guid farmId)
        {
            await FarmService.Delete(farmId, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner")]
        public async Task<IActionResult> Update([FromForm] UpdateFarmDto dto)
        {
            await FarmService.Update(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateFarmCategoriesAndSubcategories([FromBody] UpdateFarmCategoriesAndSubcategoriesDto dto)
        {
            await FarmService.UpdateFarmCategoriesAndSubcategories(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateFarmPaymentDataDto dto)
        {
            await FarmService.UpdatePaymentData(dto, AccountId);
            return NoContent();
        }
    }
}
