using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }
        public DateTime ReceiveDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ReceivingMethod ReceivingType { get; set; }
        public OrderStatus Status { get; set; }
        public AddressDto? DeliveryAddress { get; set; }
        public List<OrderItemDto>? Items { get; set; }
    }

}
