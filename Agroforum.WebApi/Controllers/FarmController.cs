using Agroforum.Application.DataTransferObjects.Farm;
using Agroforum.Application.Services.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agroforum.WebApi.Controllers
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
        public async Task<IActionResult> Get([FromRoute] Guid farmId)
        {
            var request = await FarmService.Get(farmId);
            return Ok(request);
        }

        [HttpGet]
        [Authorize(Roles = "FarmOwner")]
        public async Task<IActionResult> GetAll()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var request = await FarmService.GetAll(userId);
            return Ok(request);
        }

        [HttpPost]
        [Authorize(Roles = "FarmOwner")]
        public async Task<IActionResult> Create([FromBody] CreateFarmDto createFarmDto)
        {
            createFarmDto.OwnerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await FarmService.Create(createFarmDto);
            return NoContent();
        }

        [HttpDelete("{farmId}")]
        [Authorize(Roles = "FarmOwner")]
        public async Task<IActionResult> Delete([FromRoute] Guid farmId)
        {
            var ownerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await FarmService.Delete(farmId, ownerId);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFarmDto updateFarmDto)
        {
            await FarmService.Update(updateFarmDto);
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFarmImages([FromBody] UpdateFarmImagesDto updateFarmImagesDto)
        {
            await FarmService.UpdateFarmImages(updateFarmImagesDto);
            return NoContent();
        }
    }
}
