using Microsoft.AspNetCore.Mvc;
using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Application.ViewModels.Order;
using System.Security.Claims;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService OrderService;
        private Guid AccountId => Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            OrderService = orderService;
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderVm), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid orderId)
        {
            var vm = await OrderService.Get(orderId);
            return Ok(vm);
        }

        [HttpGet]
        [ProducesResponseType(typeof(OrderListVm), 200)]
        public async Task<IActionResult> GetAll([FromQuery] GetOrderListDto dto)
        {
            var vm = await OrderService.GetAll(dto);
            return Ok(vm);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> ExportToExcel([FromQuery] ExportOrdersDto dto)
        {
            string contentType = "application/octet-stream";

            (string fileName ,byte[] bytes) file = await OrderService.ExportToExcel(dto);

            return File(file.bytes, contentType, file.fileName);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Duplicate([FromBody] OrderListDto dto)
        {
            await OrderService.Duplicate(dto, AccountId);

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromQuery] OrderListDto dto)
        {
            await OrderService.Delete(dto, AccountId);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDto dto)
        {
            await OrderService.Update(dto, AccountId);

            return NoContent();
        }

        [HttpPut("{status}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> ChangeStatus([FromBody] OrderListDto dto, [FromRoute] OrderStatus status)
        {
            await OrderService.ChangeStatus(dto, status, AccountId);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddOrderItem([FromBody] AddOrderItemDto dto)
        {
            await OrderService.AddOrderItem(dto, AccountId);

            return NoContent();
        }
    }
}
