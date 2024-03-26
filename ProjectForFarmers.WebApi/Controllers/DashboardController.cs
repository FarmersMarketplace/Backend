using Microsoft.AspNetCore.Mvc;
using FarmersMarketplace.Application.DataTransferObjects.Dashboard;
using FarmersMarketplace.Application.Services.Business;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService DashboardService;

        public DashboardController(IDashboardService dashboardService, IConfiguration configuration)
        {
            DashboardService = dashboardService;
        }

        [HttpGet("{dashboardId}")]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        public async Task<IActionResult> Get([FromRoute] Guid dashboardId)
        {
            var vm = await DashboardService.Get(dashboardId);
            return Ok(vm);
        }

        [HttpGet("{producer}/{producerId}")]
        [ProducesResponseType(typeof(LoadDashboardVm), 200)]
        public async Task<IActionResult> Load([FromRoute] Guid producerId, [FromRoute] Producer producer)
        {
            var vm = await DashboardService.Load(producerId, producer);
            return Ok(vm);
        }

        [HttpGet("{producer}/{producerId}")]
        [ProducesResponseType(typeof(DashboardVm), 200)]
        public async Task<IActionResult> GetCurrentMonth([FromRoute] Guid producerId, [FromRoute] Producer producer)
        {
            var vm = await DashboardService.GetCurrentMonth(producerId, producer);
            return Ok(vm);
        }

        [HttpGet("{producer}/{producerId}/{query}/{count}")]
        [ProducesResponseType(typeof(LoadDashboardVm), 200)]
        public async Task<IActionResult> CustomerAutocomplete([FromRoute] Guid producerId, [FromRoute] Producer producer,
            [FromRoute] string query, [FromRoute] int count)
        {
            var vm = await DashboardService.CustomerAutocomplete(producerId, producer, query, count);
            return Ok(vm);
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerInfoVm), 200)]
        public async Task<IActionResult> GetCustomer([FromQuery] GetCustomerDto dto)
        {
            var vm = await DashboardService.SearchCustomer(dto);
            return Ok(vm);
        }
    }
}
