using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IOrderService
    {
        public Task<LoadDashboardVm> LoadDashboard(Guid producerId, Producer producer);
        public Task<OrderListVm> GetAll(Guid producerId, Producer producer);
        public Task<DashboardVm> GetDashboard(Guid Id);
        public Task<string> ExportToExcel(Guid producerId, Producer producer);
        public Task<DashboardVm> GetCurrentMonthDashboard(Guid producerId, Producer producer);
        public Task Duplicate(OrderListDto orderListDto);
        public Task Delete(OrderListDto orderListDto);
    }

}
