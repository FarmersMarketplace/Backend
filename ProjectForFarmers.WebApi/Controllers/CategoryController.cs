using Microsoft.AspNetCore.Mvc;
using ProjectForFarmers.Application.DataTransferObjects.Catefory;
using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.Services.Business;
using ProjectForFarmers.Application.ViewModels.Category;
using ProjectForFarmers.Application.ViewModels.Farm;

namespace ProjectForFarmers.WebApi.Controllers
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

