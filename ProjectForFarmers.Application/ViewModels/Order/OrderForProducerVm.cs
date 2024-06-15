using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Application.ViewModels.Order
{
    public class OrderForProducerVm
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public decimal TotalPayment { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ReceivingMethod ReceivingMethod { get; set; }
        public CustomerAddressVm? DeliveryPoint { get; set; }
        public OrderStatus Status { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAdditionalPhone { get; set; }
        public List<OrderItemVm> Items { get; set; }
    }

}
