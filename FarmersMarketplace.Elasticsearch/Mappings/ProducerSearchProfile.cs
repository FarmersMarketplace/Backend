using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Order;
using FarmersMarketplace.Application.ViewModels.Producers;
using FarmersMarketplace.Elasticsearch.Documents;

namespace FarmersMarketplace.Elasticsearch.Mappings
{
    public class ProducerSearchProfile : Profile
    {
        public ProducerSearchProfile()
        {
            MapProducerDocmentToProducerLookupVm();
        }

        private void MapProducerDocmentToProducerLookupVm()
        {
            CreateMap<ProducerDocument, ProducerLookupVm>();
        }
    }
}
