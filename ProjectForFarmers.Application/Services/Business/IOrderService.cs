using ProjectForFarmers.Application.DataTransferObjects.Order;
using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.Services.Business
{
    public interface IOrderService
    {
        public Task<OrderVm> Get(Guid orderId);
        public Task<LoadDashboardVm> LoadDashboard(Guid producerId, Producer producer);
        public Task<OrderListVm> GetAll(Guid producerId, Producer producer, OrderFilter filter);
        public Task<DashboardVm> GetDashboard(Guid Id);
        public Task<string> ExportToExcel(Guid producerId, Producer producer);
        public Task<DashboardVm> GetCurrentMonthDashboard(Guid producerId, Producer producer);
        public Task Duplicate(OrderListDto orderListDto);
        public Task Delete(OrderListDto orderListDto);
        public Task Update(UpdateOrderDto updateOrderDto);
        public Task AddOrderItem(AddOrderItemDto addOrderItemDto);
    }

}
