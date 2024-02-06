using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ProjectForFarmers.Application.ViewModels.Farm;

namespace ProjectForFarmers.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FarmController : ControllerBase
    {
        private readonly IFarmService FarmService;

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
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var request = await FarmService.GetAll(userId);
            return Ok(request);
        }

        [HttpPost]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromForm] CreateFarmDto createFarmDto)
        {
            createFarmDto.OwnerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await FarmService.Create(createFarmDto);
            return NoContent();
        }

        [HttpDelete("{farmId}")]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromRoute] Guid farmId)
        {
            var ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await FarmService.Delete(farmId, ownerId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner")]
        public async Task<IActionResult> Update([FromBody] UpdateFarmDto updateFarmDto)
        {
            await FarmService.Update(updateFarmDto);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateFarmCategoriesAndSubcategories([FromBody] UpdateFarmCategoriesAndSubcategoriesDto updateFarmCategoriesAndSubcategoriesDto)
        {
            await FarmService.UpdateFarmCategoriesAndSubcategories(updateFarmCategoriesAndSubcategoriesDto);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateFarmSettingsDto updateFarmSettingsDto)
        {
            await FarmService.UpdateSettings(updateFarmSettingsDto);
            return NoContent();
        }
    }
}
