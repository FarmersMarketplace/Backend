using Microsoft.AspNetCore.Mvc;
using FarmersMarketplace.Application.DataTransferObjects.Subcategory;
using FarmersMarketplace.Application.Services.Business;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService SubcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            SubcategoryService = subcategoryService;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromForm] CreateSubcategoryDto dto)
        {
            await SubcategoryService.Create(dto);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update([FromBody] UpdateSubcategoryDto dto)
        {
            await SubcategoryService.Update(dto);
            return NoContent();
        }

        [HttpDelete("{subcategoryId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromRoute] Guid subcategoryId)
        {
            await SubcategoryService.Delete(subcategoryId);
            return NoContent();
        }
    }
}
