using AutoMapper;

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
        }
    }


}
