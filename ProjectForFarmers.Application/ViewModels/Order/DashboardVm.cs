using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Order
{
    public class DashboardVm
    {
        public OrderGroupStatisticVm BookedOrders { get; set; }
        public OrderGroupStatisticVm CompletedOrders { get; set; }
        public OrderGroupStatisticVm ProcessingOrders { get; set; }
        public OrderGroupStatisticVm NewOrders { get; set; }
        public OrderGroupStatisticVm TotalActivity { get; set; }
        public decimal TotalRevenue { get; set; }
        public float TotalRevenueChangePercentage { get; set; }
        public decimal HighestCustomerPayment { get; set; }
        public float HighestCustomerPaymentPercentage { get; set; }
        public string CustomerWithHighestPaymentName { get; set; }
    }

}
