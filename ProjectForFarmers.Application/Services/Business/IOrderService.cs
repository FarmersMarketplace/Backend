﻿using FarmersMarketplace.Application.DataTransferObjects.Order;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Domain.Orders;

namespace FarmersMarketplace.Application.Services.Business
{
    public interface IOrderService
    {
       Task Create(CreateOrderDto dto);
       Task<OrderForProducerVm> GetForProducer(Guid orderId);
       Task<OrderForCustomerVm> GetForCustomer(Guid orderId);
       Task<(string fileName, byte[] bytes)> ExportToExcel(ExportOrdersDto dto);
       Task Duplicate(OrderListDto dto, Guid accountId);
       Task Delete(OrderListDto dto, Guid accountId);
       Task ChangeStatus(OrderListDto dto, OrderStatus status, Guid accountId);
       Task Update(UpdateOrderDto dto, Guid accountId);
       Task AddOrderItem(AddOrderItemDto dto, Guid accountId);
    }

}
