using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using Nest;

namespace FarmersMarketplace.Elasticsearch.Factories
{
    public interface IIndexFactory
    {
        void CreateIndex(IElasticClient client);
        CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor descriptor);
        Task LoadData(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper);
    }
}
