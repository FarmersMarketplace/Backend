using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService ProductService;
        private readonly ISearchProvider<GetProducerProductListDto, ProducerProductListVm, ProducerProductAutocompleteDto> ProducerSearchProvider;
        private readonly ISearchProvider<GetCustomerProductListDto, CustomerProductListVm, CustomerProductAutocompleteDto> CustomerSearchProvider;
        private Guid AccountId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public ProductController(IProductService productService, 
            ISearchProvider<GetProducerProductListDto, ProducerProductListVm, ProducerProductAutocompleteDto> producerSearchProvider,
            ISearchProvider<GetCustomerProductListDto, CustomerProductListVm, CustomerProductAutocompleteDto> customerSearchProvider)
        {
            ProductService = productService;
            ProducerSearchProvider = producerSearchProvider;
            CustomerSearchProvider = customerSearchProvider;
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(typeof(ProductForProducerVm), 200)]
        public async Task<IActionResult> GetForProducer([FromRoute] Guid productId)
        {
            var request = await ProductService.GetForProducer(productId);
            return Ok(request);
        }

        [HttpGet("{product}/{productId}")]
        [ProducesResponseType(typeof(ProductForProducerVm), 200)]
        public async Task<IActionResult> GetProducerProductFilterData([FromRoute] Guid productId, [FromRoute] Producer producer)
        {
            var request = await ProductService.GetProducerProductFilterData(producer, productId);
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
        [ProducesResponseType(typeof(ProducerProductListVm), 200)]
        public async Task<IActionResult> GetAllForProducer([FromQuery] GetProducerProductListDto dto)
        {
            var request = await ProducerSearchProvider.Search(dto);
            return Ok(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerProductListVm), 200)]
        public async Task<IActionResult> GetAllForCustomer([FromBody] GetCustomerProductListDto dto)
        {
            var request = await CustomerSearchProvider.Search(dto);
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
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IActionResult> AutocompleteForCustomer([FromQuery] CustomerProductAutocompleteDto dto)
        {
            var request = await CustomerSearchProvider.Autocomplete(dto);
            return Ok(request);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), 200)]
        public async Task<IActionResult> AutocompleteForProducer([FromQuery] ProducerProductAutocompleteDto dto)
        {
            var request = await ProducerSearchProvider.Autocomplete(dto);
            return Ok(request);
        }
    }
}
