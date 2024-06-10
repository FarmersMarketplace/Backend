using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Elasticsearch.Documents;

namespace FarmersMarketplace.Elasticsearch.Mappings
{
    public class OrderSearchProfile : Profile
    {
        public OrderSearchProfile()
        {
            MapOrderToOrderDocument();
            MapOrderDocumentToProducerOrderLookupVm();
        }

        private void MapOrderDocumentToProducerOrderLookupVm()
        {
            CreateMap<OrderDocument, ProducerOrderLookupVm>()
                .ForMember(vm => vm.Number, opt => opt.MapFrom(document => document.Number.ToString()));
        }

        private void MapOrderToOrderDocument()
        {
        }
    }


}
