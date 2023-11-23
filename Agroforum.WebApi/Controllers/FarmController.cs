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
        private IFarmService FarmService;

        public FarmController(IFarmService farmService)
        {
            FarmService = farmService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromBody] GetFarmDto getFarmDto)
        {
            var request = await FarmService.Get(getFarmDto);
            return Ok(request);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var request = await FarmService.GetAll(userId);
            return Ok(request);
        }

        [Authorize(Roles = "Farmer, User")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFarmDto createFarmDto)
        {
            createFarmDto.OwnerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await FarmService.Create(createFarmDto);
            return NoContent();
        }

        [Authorize(Roles = "Farmer, User")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteFarmDto deleteFarmDto)
        {
            await FarmService.Delete(deleteFarmDto);
            return NoContent();
        }

        [Authorize(Roles = "Farmer, User")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateFarmDto updateFarmDto)
        {
            await FarmService.Update(updateFarmDto);
            return NoContent();
        }

        [Authorize(Roles = "Farmer, User")]
        [HttpPut]
        public async Task<IActionResult> UpdateImages([FromBody] UpdateFarmImagesDto updateFarmDto)
        {
            await FarmService.Update(updateFarmDto);
            return NoContent();
        }
    }
}
