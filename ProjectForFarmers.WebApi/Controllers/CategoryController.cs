﻿using FarmersMarketplace.Application.DataTransferObjects.Catefory;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Domain;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Create([FromBody] CategoryDto dto)
        {
            await CategoryService.Create(dto);
            return NoContent();
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update([FromRoute] Guid categoryId,[FromBody] CategoryDto dto)
        {
            await CategoryService.Update(categoryId, dto);
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

        [HttpGet("{producerId}/{producer}")]
        [ProducesResponseType(typeof(CategoriesAndSubcategoriesVm), 200)]
        public async Task<IActionResult> GetProducerData([FromRoute] Guid producerId,[FromRoute] Producer producer)
        {
            var vm = await CategoryService.GetProducerData(producerId, producer);
            return Ok(vm);
        }
    }
}

