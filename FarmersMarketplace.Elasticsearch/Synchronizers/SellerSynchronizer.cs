using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;

namespace FarmersMarketplace.Elasticsearch.Synchronizers
{
    internal class SellerSynchronizer : ISearchSynchronizer<Seller>
    {
        private readonly IElasticClient Client;
        private readonly IMapper Mapper;

        public SellerSynchronizer(IElasticClient client, IMapper mapper)
        {
            Client = client;
            Mapper = mapper;
        }

        public async Task Create(Seller obj)
        {
            var document = Mapper.Map<ProducerDocument>(obj);
            await Client.IndexDocumentAsync(document);
        }

        public async Task Delete(Guid id)
        {
            await Client.DeleteAsync<ProducerDocument>(id);
        }

        public async Task Update(Seller obj)
        {
            var document = Mapper.Map<ProducerDocument>(obj);
            await Client.UpdateAsync<ProducerDocument, object>(obj.Id, u => u
                .Doc(document)
                .Index(Indecies.Producers));
        }
    }
}
