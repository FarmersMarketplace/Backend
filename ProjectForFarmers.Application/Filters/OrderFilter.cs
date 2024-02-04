using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Filters
{
    public class OrderFilter : IFilter<List<Order>>
    {
        public List<OrderStatus>? Statuses { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }

        public async Task Filter(List<Order> collection)
        {
            collection = collection
            .Where(o =>
                (Statuses == null || !Statuses.Any() || Statuses.Contains(o.Status)) &&
                (!StartDate.HasValue || o.CreationDate >= StartDate.GetValueOrDefault(DateTime.MinValue)) &&
                (!EndDate.HasValue || o.CreationDate <= EndDate.GetValueOrDefault(DateTime.MaxValue)) &&
                (PaymentTypes == null || !PaymentTypes.Any() || PaymentTypes.Contains(o.PaymentType)))
            .ToList();
        }
    }

}
