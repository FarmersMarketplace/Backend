﻿using Microsoft.AspNetCore.Mvc;
using ProjectForFarmers.Application.DataTransferObjects.Subcategory;
using ProjectForFarmers.Application.Services.Business;

namespace ProjectForFarmers.WebApi.Controllers
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
        public async Task<IActionResult> Create(CreateSubcategoryDto createSubcategoryDto)
        {
            await SubcategoryService.Create(createSubcategoryDto);
            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update(UpdateSubcategoryDto updateSubcategoryDto)
        {
            await SubcategoryService.Update(updateSubcategoryDto);
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