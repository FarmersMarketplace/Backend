using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Domain.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Filters
{
    public class OrderFilter
    {
        public List<OrderStatus>? Statuses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }
    }

}
