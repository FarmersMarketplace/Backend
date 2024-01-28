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
        }

        private void MapMonthStatisticToDashboardVm()
        {
            CreateMap<MonthStatistic, DashboardVm>()
                .ForMember(vm => vm.BookedOrders, opt => opt.MapFrom(statistic => statistic.BookedOrders))
                .ForMember(vm => vm.CompletedOrders, opt => opt.MapFrom(statistic => statistic.CompletedOrders))
                .ForMember(vm => vm.ProcessingOrders, opt => opt.MapFrom(statistic => statistic.ProcessingOrders))
                .ForMember(vm => vm.NewOrders, opt => opt.MapFrom(statistic => statistic.NewOrders))
                .ForMember(vm => vm.TotalActivity, opt => opt.MapFrom(statistic => statistic.TotalActivity))
                .ForMember(vm => vm.TotalRevenue, opt => opt.MapFrom(statistic => statistic.TotalRevenue))
                .ForMember(vm => vm.TotalRevenueChangePercentage, opt => opt.MapFrom(statistic => statistic.TotalRevenueChangePercentage))
                .ForMember(vm => vm.HighestCustomerPayment, opt => opt.MapFrom(statistic => statistic.HighestCustomerPayment))
                .ForMember(vm => vm.HighestCustomerPaymentPercentage, opt => opt.MapFrom(statistic => statistic.HighestCustomerPaymentPercentage))
                .ForMember(vm => vm.CustomerWithHighestPaymentName, opt => opt.MapFrom(statistic => ""));
        }

        private void MapOrderGroupStatisticToOrderGroupStatisticVm()
        {
            CreateMap<OrderGroupStatistic, OrderGroupStatisticVm>()
                .ForMember(vm => vm.Count, opt => opt.MapFrom(statistic => statistic.Count))
                .ForMember(vm => vm.PercentageChange, opt => opt.MapFrom(statistic => statistic.PercentageChange));
        }
    }

}
