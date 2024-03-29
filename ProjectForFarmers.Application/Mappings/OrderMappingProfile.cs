using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.ViewModels.Dashboard;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Mappings
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            MapOrderGroupStatisticToOrderGroupStatisticVm();
            MapMonthStatisticToDashboardVm();
            MapOrderToOrderLookupVm();
            MapOrderToOrderVm();
        }

        private void MapMonthStatisticToDashboardVm()
        {
            CreateMap<MonthStatistic, DashboardVm>()
                .ForMember(vm => vm.BookedOrders, opt => opt.MapFrom(statistic => statistic.BookedOrdersStatistic))
                .ForMember(vm => vm.CompletedOrders, opt => opt.MapFrom(statistic => statistic.CompletedOrdersStatistic))
                .ForMember(vm => vm.ProcessingOrders, opt => opt.MapFrom(statistic => statistic.ProcessingOrdersStatistic))
                .ForMember(vm => vm.NewOrders, opt => opt.MapFrom(statistic => statistic.NewOrdersStatistic))
                .ForMember(vm => vm.TotalActivity, opt => opt.MapFrom(statistic => statistic.TotalActivityStatistic))
                .ForMember(vm => vm.TotalRevenueChangePercentage, opt => opt.MapFrom(statistic => statistic.TotalRevenueChangePercentage))
                .ForMember(vm => vm.CustomerInfo, opt => opt.MapFrom(statistic => new CustomerInfoVm()));
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
                .ForMember(vm => vm.CustomerName, opt => opt.MapFrom(order => order.Customer.Name + " " + order.Customer.Surname))
                .ForMember(vm => vm.CustomerPhone, opt => opt.MapFrom(order => order.Customer.Phone))
                .ForMember(vm => vm.CustomerEmail, opt => opt.MapFrom(order => order.Customer.Email))
                .ForMember(vm => vm.TotalPayment, opt => opt.MapFrom(order => order.TotalPayment))
                .ForMember(vm => vm.PaymentType, opt => opt.MapFrom(order => order.PaymentType))
                .ForMember(vm => vm.Status, opt => opt.MapFrom(order => order.Status));
        }

        private void MapOrderToOrderVm()
        {
            CreateMap<Order, OrderVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(order => order.Id))
                .ForMember(vm => vm.Number, opt => opt.MapFrom(order => order.Number.ToString("D7")))
                .ForMember(vm => vm.CreationDate, opt => opt.MapFrom(order => order.CreationDate))
                .ForMember(vm => vm.ReceiveDate, opt => opt.MapFrom(order => order.ReceiveDate))
                .ForMember(vm => vm.TotalPayment, opt => opt.MapFrom(order => order.TotalPayment))
                .ForMember(vm => vm.PaymentType, opt => opt.MapFrom(order => order.PaymentType))
                .ForMember(vm => vm.PaymentStatus, opt => opt.MapFrom(order => order.PaymentStatus))
                .ForMember(vm => vm.ReceivingMethod, opt => opt.MapFrom(order => order.ReceivingMethod))
                .ForMember(vm => vm.DeliveryPoint, opt => opt.MapFrom(order => order.DeliveryPoint))
                .ForMember(vm => vm.Status, opt => opt.MapFrom(order => order.Status))
                .ForMember(vm => vm.CustomerId, opt => opt.MapFrom(order => order.CustomerId))
                .ForMember(vm => vm.CustomerName, opt => opt.MapFrom(order => order.Customer.Name + " " + order.Customer.Surname))
                .ForMember(vm => vm.CustomerPhone, opt => opt.MapFrom(order => order.Customer.Phone))
                .ForMember(vm => vm.Items, opt => opt.MapFrom(order => new List<OrderItemVm>()));
        }
    }

}
