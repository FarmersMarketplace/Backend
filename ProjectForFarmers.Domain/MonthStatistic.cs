using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class MonthStatistic
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public OrderGroupStatistic BookedOrders { get; set; }
        public Guid BookedOrdersStatisticId { get; set; }
        public OrderGroupStatistic CompletedOrders { get; set; }
        public Guid CompleteOrdersStatisticId { get; set; }
        public OrderGroupStatistic ProcessingOrders { get; set; }
        public Guid ProcessingOrdersStatisticId { get; set; }
        public OrderGroupStatistic NewOrders { get; set; }
        public Guid NewOrdersStatisticId { get; set; }
        public OrderGroupStatistic PreviousOrders { get; set; }
        public Guid PreviousOrdersStatisticId { get; set; }
        public OrderGroupStatistic TotalActivity { get; set; }
        public Guid TotalActivityStatisticId { get; set; }
        public decimal TotalRevenue { get; set; }
        public float TotalRevenuePercentage { get; set; }
    }

}
