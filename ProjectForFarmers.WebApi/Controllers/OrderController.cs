using Microsoft.AspNetCore.Mvc;
using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Application.Services.Business;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService OrderService;
        private readonly IConfiguration Configuration;

        public OrderController(IOrderService orderService, IConfiguration configuration)
        {
            OrderService = orderService;
            Configuration = configuration;
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(OrderVm), 200)]
        public async Task<IActionResult> LoadDashboard([FromRoute] Guid orderId)
        {
            var vm = await OrderService.Get(orderId);
            return Ok(vm);
        }

        [HttpGet("{producerId}/{producer}")]
        [ProducesResponseType(typeof(LoadDashboardVm), 200)]
        public async Task<IActionResult> LoadDashboard([FromRoute] Guid producerId, [FromRoute] Producer producer)
        {
            var vm = await OrderService.LoadDashboard(producerId, producer);
            return Ok(vm);
        }

        [HttpGet]
        [ProducesResponseType(typeof(OrderListVm), 200)]
        public async Task<IActionResult> GetAll([FromBody] GetOrderListDto getOrderListDto)
        {
            var vm = await OrderService.GetAll(getOrderListDto);
            return Ok(vm);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        public async Task<IActionResult> GetDashboard([FromRoute] Guid id)
        {
            var vm = await OrderService.GetDashboard(id);
            return Ok(vm);
        }

        [HttpGet("{producerId}/{producer}")]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> ExportToExcel([FromRoute] Guid producerId, [FromRoute] Producer producer)
        {
            string fileName = await OrderService.ExportToExcel(producerId, producer);
            string filePath = Configuration["Files"] + "\\fileName";
            string contentType = "application/octet-stream";

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            System.IO.File.Delete(filePath);

            return File(fileBytes, contentType, fileName);
        }

        [HttpGet("{producerId}/{producer}")]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        public async Task<IActionResult> GetCurrentMonthDashboard([FromRoute] Guid producerId, [FromRoute] Producer producer) 
        {
            var vm = await OrderService.GetCurrentMonthDashboard(producerId, producer);

            return Ok(vm);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Duplicate([FromBody] OrderListDto orderListDto)
        {
            await OrderService.Duplicate(orderListDto);

            return NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete([FromBody] OrderListDto orderListDto)
        {
            await OrderService.Delete(orderListDto);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDto updateOrderDto)
        {
            await OrderService.Update(updateOrderDto);

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(204)]
        public async Task<IActionResult> AddOrderItem([FromBody] AddOrderItemDto addOrderItemDto)
        {
            await OrderService.AddOrderItem(addOrderItemDto);

            return NoContent();
        }
    }
}
