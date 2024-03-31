using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Application.Filters
{
    public class ProducerOrderFilter
    {
        public List<OrderStatus>? Statuses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
    }

}
