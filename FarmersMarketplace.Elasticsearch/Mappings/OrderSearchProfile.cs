using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Elasticsearch.Documents;

namespace FarmersMarketplace.Elasticsearch.Mappings
{
    public class OrderSearchProfile : Profile
    {
        public OrderSearchProfile()
        {
            MapOrderToOrderDocument();
            MapOrderDocumentToProducerOrderLookupVm();
            MapOrderDocumentToCustomerOrderLookupVm();
        }

        private void MapOrderDocumentToCustomerOrderLookupVm()
        {
            CreateMap<OrderDocument, CustomerOrderLookupVm>();
        }

        private void MapOrderDocumentToProducerOrderLookupVm()
        {
            CreateMap<OrderDocument, ProducerOrderLookupVm>()
                .ForMember(vm => vm.CustomerName, opt => opt.MapFrom(document => document.CustomerName + " " + document.CustomerSurname));
        }

        private void MapOrderToOrderDocument()
        {
            CreateMap<Order, OrderDocument>()
                .ForMember(document => document.Number, opt => opt.MapFrom(order => order.Number.ToString("D7")))
                .ForMember(document => document.CustomerEmail, opt => opt.MapFrom(order => order.Customer.Email));
        }
    }


}
