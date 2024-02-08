using ProjectForFarmers.Application.ViewModels.Order;

namespace ProjectForFarmers.Application.ViewModels.Dashboard
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
        public CustomerInfoVm CustomerInfo { get; set; }
    }

}
