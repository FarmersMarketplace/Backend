using Microsoft.AspNetCore.Mvc;
using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.Services.Business;
using ProjectForFarmers.Application.ViewModels.Dashboard;
using ProjectForFarmers.Application.ViewModels.Order;
using System.Security.Claims;

namespace ProjectForFarmers.WebApi.Controllers
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
        public async Task<IActionResult> GetAll([FromQuery] GetOrderListDto getOrderListDto)
        {
            var vm = await OrderService.GetAll(getOrderListDto);
            return Ok(vm);
        }

        [HttpGet]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> ExportToExcel([FromQuery] ExportOrdersDto exportOrdersDto)
        {
            string contentType = "application/octet-stream";

            (string fileName ,byte[] bytes) file = await OrderService.ExportToExcel(exportOrdersDto);

            return File(file.bytes, contentType, file.fileName);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Duplicate([FromBody] OrderListDto orderListDto)
        {
            await OrderService.Duplicate(orderListDto, AccountId);

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromBody] OrderListDto orderListDto)
        {
            await OrderService.Delete(orderListDto, AccountId);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDto updateOrderDto)
        {
            await OrderService.Update(updateOrderDto, AccountId);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddOrderItem([FromBody] AddOrderItemDto addOrderItemDto)
        {
            await OrderService.AddOrderItem(addOrderItemDto, AccountId);

            return NoContent();
        }
    }
}
