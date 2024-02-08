using Microsoft.AspNetCore.Mvc;
using ProjectForFarmers.Application.Services.Business;

namespace ProjectForFarmers.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DashboardController
    {
        private readonly IDashboardService DashboardService;
        private readonly IConfiguration Configuration;

        public DashboardController(IDashboardService dashboardService, IConfiguration configuration)
        {
            DashboardService = dashboardService;
            Configuration = configuration;
        }


    }
}
