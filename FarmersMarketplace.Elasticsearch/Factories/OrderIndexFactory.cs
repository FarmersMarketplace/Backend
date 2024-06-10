using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using Nest;

namespace FarmersMarketplace.Elasticsearch.Factories
{
    public class OrderIndexFactory : IIndexFactory
    {
        public CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor descriptor)
        {
            throw new NotImplementedException();
        }

        public void CreateIndex(IElasticClient client)
        {
            string indexName = Indecies.Orders;

            if (!client.Indices.Exists(indexName).Exists)
            {
                var descriptor = new CreateIndexDescriptor(indexName);
                descriptor = ConfigureIndex(descriptor);
                client.Indices.Create(indexName, c => descriptor);
            }
        }

        public Task LoadData(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper)
        {
            throw new NotImplementedException();
        }
    }
}
