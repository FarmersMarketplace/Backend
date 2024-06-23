using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Orders;

namespace FarmersMarketplace.Application.ViewModels.Order
{
    public class OrderForCustomerVm
    {
        public Guid Id { get; set; }
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public string ProducerImageName { get; set; }
        public string ProducerName { get; set; }
        public AddressVm ProducerAddress { get; set; }
        public string Number { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemVm> Items { get; set; }
        public decimal TotalPayment { get; set; }
    }
}
