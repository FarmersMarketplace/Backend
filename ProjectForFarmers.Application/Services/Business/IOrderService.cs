using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.ViewModels.Order;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IOrderService
    {
       Task<OrderVm> Get(Guid orderId);
       Task<OrderListVm> GetAll(GetOrderListDto getOrderListDto);
       Task<(string fileName, byte[] bytes)> ExportToExcel(ExportOrdersDto exportOrdersDto);
       Task Duplicate(OrderListDto orderListDto, Guid accountId);
       Task Delete(OrderListDto orderListDto, Guid accountId);
       Task Update(UpdateOrderDto updateOrderDto, Guid accountId);
       Task AddOrderItem(AddOrderItemDto addOrderItemDto, Guid accountId);
    }

}
