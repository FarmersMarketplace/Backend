using AutoMapper;
using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.ViewModels.Order;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            MapOrderGroupStatisticToOrderGroupStatisticVm();
            MapMonthStatisticToDashboardVm();
            MapOrderToOrderLookupVm();
        }

        private void MapMonthStatisticToDashboardVm()
        {
            CreateMap<MonthStatistic, DashboardVm>()
                .ForMember(vm => vm.BookedOrders, opt => opt.MapFrom(statistic => statistic.BookedOrdersStatistic))
                .ForMember(vm => vm.CompletedOrders, opt => opt.MapFrom(statistic => statistic.CompletedOrdersStatistic))
                .ForMember(vm => vm.ProcessingOrders, opt => opt.MapFrom(statistic => statistic.ProcessingOrdersStatistic))
                .ForMember(vm => vm.NewOrders, opt => opt.MapFrom(statistic => statistic.NewOrdersStatistic))
                .ForMember(vm => vm.TotalActivity, opt => opt.MapFrom(statistic => statistic.TotalActivityStatistic))
                .ForMember(vm => vm.TotalRevenue, opt => opt.MapFrom(statistic => statistic.TotalRevenue))
                .ForMember(vm => vm.TotalRevenueChangePercentage, opt => opt.MapFrom(statistic => statistic.TotalRevenueChangePercentage))
                .ForMember(vm => vm.HighestCustomerPayment, opt => opt.MapFrom(statistic => statistic.HighestCustomerPayment))
                .ForMember(vm => vm.HighestCustomerPaymentPercentage, opt => opt.MapFrom(statistic => statistic.HighestCustomerPaymentPercentage))
                .ForMember(vm => vm.CustomerWithHighestPaymentName, opt => opt.MapFrom(statistic => ""));
        }

        private void MapOrderGroupStatisticToOrderGroupStatisticVm()
        {
            CreateMap<OrderGroupStatistic, OrderGroupStatisticVm>()
                .ForMember(vm => vm.Count, opt => opt.MapFrom(order => order.Count))
                .ForMember(vm => vm.PercentageChange, opt => opt.MapFrom(order => order.PercentageChange));
        }

        private void MapOrderToOrderLookupVm()
        {
            CreateMap<Order, OrderLookupVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(order => order.Id))
                .ForMember(vm => vm.Number, opt => opt.MapFrom(order => order.Number.ToString("D7")))
                .ForMember(vm => vm.CreationDate, opt => opt.MapFrom(order => order.CreationDate))
                .ForMember(vm => vm.CustomerName, opt => opt.MapFrom(order => order.CustomerName))
                .ForMember(vm => vm.CustomerPhone, opt => opt.MapFrom(order => order.CustomerPhone))
                .ForMember(vm => vm.CustomerEmail, opt => opt.MapFrom(order => order.CustomerEmail))
                .ForMember(vm => vm.TotalPayment, opt => opt.MapFrom(order => order.TotalPayment))
                .ForMember(vm => vm.PaymentType, opt => opt.MapFrom(order => order.TotalPayment))
                .ForMember(vm => vm.Status, opt => opt.MapFrom(order => order.Status));
        }
    }

}
