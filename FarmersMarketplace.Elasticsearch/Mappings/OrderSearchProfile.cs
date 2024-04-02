using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Elasticsearch.Documents;

namespace FarmersMarketplace.Elasticsearch.Mappings
{
    public class OrderSearchProfile : Profile
    {
        public OrderSearchProfile()
        {
            MapOrderToOrderDocument();
        }

        private void MapOrderToOrderDocument()
        {
            CreateMap<OrderDocument, ProducerOrderLookupVm>();
        }
    }


}
