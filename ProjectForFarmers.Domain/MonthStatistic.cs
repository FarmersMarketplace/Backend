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
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid BookedOrdersStatisticId { get; set; }
        public Guid CompletedOrdersStatisticId { get; set; }
        public Guid ProcessingOrdersStatisticId { get; set; }
        public Guid NewOrdersStatisticId { get; set; }
        public Guid TotalActivityStatisticId { get; set; }
        public OrderGroupStatistic BookedOrdersStatistic { get; set; }
        public OrderGroupStatistic CompletedOrdersStatistic { get; set; }
        public OrderGroupStatistic ProcessingOrdersStatistic { get; set; }
        public OrderGroupStatistic NewOrdersStatistic { get; set; }
        public OrderGroupStatistic TotalActivityStatistic { get; set; }
        public decimal TotalRevenue { get; set; }
        public Guid? CustomerWithHighestPaymentId { get; set; }
        public float TotalRevenueChangePercentage { get; set; }
        public decimal HighestCustomerPayment { get; set; }
        public float HighestCustomerPaymentPercentage { get; set; }
    }

}
