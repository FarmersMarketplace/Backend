using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;

namespace FarmersMarketplace.Elasticsearch.Synchronizers
{
    public class OrderSynchronizer : ISearchSynchronizer<Order>
    {
        private readonly IElasticClient Client;
        private readonly IMapper Mapper;

        public OrderSynchronizer(IElasticClient client, IMapper mapper)
        {
            Client = client;
            Mapper = mapper;
        }

        public async Task Create(Order obj)
        {
            var document = Mapper.Map<OrderDocument>(obj);
            await Client.IndexDocumentAsync(document);
        }

        public async Task Delete(Guid id)
        {
            await Client.DeleteAsync<OrderDocument>(id);
        }

        public async Task Update(Order obj)
        {
            var document = Mapper.Map<OrderDocument>(obj);

            await Client.UpdateAsync<OrderDocument, object>(obj.Id, u => u
                .Doc(document)
                .Index(Indecies.Orders));
        }
    }

}
