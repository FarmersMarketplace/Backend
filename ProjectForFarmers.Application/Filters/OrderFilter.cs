using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Filters
{
    public class OrderFilter : IFilter<Order>
    {
        public List<OrderStatus>? Statuses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }
        public decimal MinimumAmount { get; set; }
        public decimal MaximumAmount { get; set; }

        public async Task<IQueryable<Order>> ApplyFilter(IQueryable<Order> query)
        {
            return query.Where(o =>
                (Statuses == null || !Statuses.Any() || Statuses.Contains(o.Status)) &&
                (!StartDate.HasValue || o.CreationDate >= StartDate) &&
                (!EndDate.HasValue || o.CreationDate <= EndDate) &&
                (PaymentTypes == null || !PaymentTypes.Any() || PaymentTypes.Contains(o.PaymentType)) &&
                (o.TotalPayment >= MinimumAmount && o.TotalPayment <= MaximumAmount));
        }
    }

}
