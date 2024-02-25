using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Application.Services.Business;
using ProjectForFarmers.Application.ViewModels.Dashboard;
using ProjectForFarmers.Application.ViewModels.Product;
using ProjectForFarmers.Domain;
using System.Security.Claims;

namespace ProjectForFarmers.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService ProductService;
        private Guid AccountId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

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

        [HttpGet("{product}/{productId}")]
        [ProducesResponseType(typeof(ProductVm), 200)]
        public async Task<IActionResult> GetFilterData([FromRoute] Guid productId, [FromRoute] Producer producer)
        {
            var request = await ProductService.GetFilterData(producer, productId);
            return Ok(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> ExportToExcel([FromQuery] ExportProductsDto exportProductsDto)
        {
            string contentType = "application/octet-stream";

            (string fileName, byte[] bytes) file = await ProductService.ExportToExcel(exportProductsDto);

            return File(file.bytes, contentType, file.fileName);
        }

        [HttpGet]
        [Authorize(Roles = "FarmOwner, Seller")]
        [ProducesResponseType(typeof(ProductListVm), 200)]
        public async Task<IActionResult> GetAll([FromQuery] GetProductListDto getProductListDto)
        {
            var request = await ProductService.GetAll(getProductListDto);
            return Ok(request);
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Duplicate([FromQuery] ProductListDto productListDto)
        {
            await ProductService.Duplicate(productListDto, AccountId);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "FarmOwner, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromForm] CreateProductDto createProductDto)
        {
            await ProductService.Create(createProductDto);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "FarmOwner, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromQuery] ProductListDto productListDto)
        {
            await ProductService.Delete(productListDto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "FarmOwner, Seller")]
        public async Task<IActionResult> Update([FromForm] UpdateProductDto updateProductDto)
        {
            await ProductService.Update(updateProductDto, AccountId);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProductVm), 200)]
        public async Task<IActionResult> Autocomplete([FromQuery] ProductAutocompleteDto productAutocompleteDto)
        {
            var request = await ProductService.Autocomplete(productAutocompleteDto);
            return Ok(request);
        }
    }
}
