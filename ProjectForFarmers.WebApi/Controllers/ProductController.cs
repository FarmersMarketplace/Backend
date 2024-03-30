using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using System.Security.Claims;

namespace FarmersMarketplace.WebApi.Controllers
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
        [ProducesResponseType(typeof(ProducerProductVm), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid productId)
        {
            var request = await ProductService.Get(productId);
            return Ok(request);
        }

        [HttpGet("{product}/{productId}")]
        [ProducesResponseType(typeof(ProducerProductVm), 200)]
        public async Task<IActionResult> GetFilterData([FromRoute] Guid productId, [FromRoute] Producer producer)
        {
            var request = await ProductService.GetFilterData(producer, productId);
            return Ok(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> ExportToExcel([FromQuery] ExportProductsDto dto)
        {
            string contentType = "application/octet-stream";

            (string fileName, byte[] bytes) file = await ProductService.ExportToExcel(dto);

            return File(file.bytes, contentType, file.fileName);
        }

        [HttpGet]
        [Authorize(Roles = "Farmer, Seller")]
        [ProducesResponseType(typeof(ProductListVm), 200)]
        public async Task<IActionResult> GetAll([FromQuery] GetProductListDto dto)
        {
            var request = await ProductService.GetAll(dto);
            return Ok(request);
        }

        [HttpPut]
        [Authorize(Roles = "Farmer, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Duplicate([FromBody] ProductListDto dto)
        {
            await ProductService.Duplicate(dto, AccountId);
            return NoContent();
        }

        [HttpPut("{status}")]
        [Authorize(Roles = "Farmer, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ChangeStatus([FromBody] ProductListDto dto, [FromRoute] ProductStatus status)
        {
            await ProductService.ChangeStatus(dto, status, AccountId);
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Farmer, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
        {
            await ProductService.Create(dto);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Farmer, Seller")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromQuery] ProductListDto dto)
        {
            await ProductService.Delete(dto, AccountId);
            return NoContent();
        }

        [HttpPut]
        [Authorize(Roles = "Farmer, Seller")]
        public async Task<IActionResult> Update([FromForm] UpdateProductDto dto)
        {
            await ProductService.Update(dto, AccountId);
            return NoContent();
        }

        [HttpGet]
        [ProducesResponseType(typeof(ProducerProductVm), 200)]
        public async Task<IActionResult> Autocomplete([FromQuery] ProductAutocompleteDto dto)
        {
            var request = await ProductService.Autocomplete(dto);
            return Ok(request);
        }
    }
}
