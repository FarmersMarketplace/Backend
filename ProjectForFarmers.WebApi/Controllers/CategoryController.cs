using Microsoft.AspNetCore.Mvc;
using FarmersMarketplace.Application.DataTransferObjects.Catefory;
using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Farm;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService CategoryService;

        public CategoryController(ICategoryService categoryService)
        {
            CategoryService = categoryService;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {
            await CategoryService.Create(categoryDto);
            return NoContent();
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(Guid categoryId, CategoryDto categoryDto)
        {
            await CategoryService.Update(categoryId, categoryDto);
            return NoContent();

        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromRoute] Guid categoryId)
        {
            await CategoryService.Delete(categoryId);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(CategoryListVm), 200)]
        public async Task<IActionResult> GetAll()
        {
            var vm = await CategoryService.GetAll();
            return Ok(vm);
        }
    }
}

