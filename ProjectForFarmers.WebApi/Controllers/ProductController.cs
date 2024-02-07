using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.Services.Business;
using ProjectForFarmers.Application.ViewModels.Product;
using System.Security.Claims;

namespace ProjectForFarmers.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService ProductService;

        public ProductController(IProductService productService)
        {
            ProductService = productService;
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(ProductVm), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid productId)
        {
            var request = await ProductService.Get(productId);
            return Ok(request);
        }

        [HttpGet]
        [Authorize(Roles = "FarmOwner, Seller")]
        [ProducesResponseType(typeof(ProductsListVm), 200)]
        public async Task<IActionResult> GetAll(GetProductListDto getProductListDto)
        {
            var request = await ProductService.GetAll(getProductListDto);
            return Ok(request);
        }

        [HttpPost]
        [Authorize(Roles = "FarmOwner, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromForm] CreateProductDto createProductDto)
        {
            await ProductService.Create(createProductDto);
            return NoContent();
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "FarmOwner, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            await ProductService.Delete(productId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner, Seller")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDto updateProductDto)
        {
            await ProductService.Update(updateProductDto);
            return NoContent();
        }
    }
}
