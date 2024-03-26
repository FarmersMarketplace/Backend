using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Order
{
    public class OrderVm
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public decimal TotalPayment { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ReceivingMethod ReceivingType { get; set; }
        public AddressVm? DeliveryPoint { get; set; }
        public OrderStatus Status { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public List<OrderItemVm> Items { get; set; }
    }

}
