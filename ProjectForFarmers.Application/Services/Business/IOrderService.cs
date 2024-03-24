using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.ViewModels.Order;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IOrderService
    {
       Task<OrderVm> Get(Guid orderId);
       Task<OrderListVm> GetAll(GetOrderListDto dto);
       Task<(string fileName, byte[] bytes)> ExportToExcel(ExportOrdersDto dto);
       Task Duplicate(OrderListDto dto, Guid accountId);
       Task Delete(OrderListDto dto, Guid accountId);
       Task Update(UpdateOrderDto dto, Guid accountId);
       Task AddOrderItem(AddOrderItemDto dto, Guid accountId);
    }

}
