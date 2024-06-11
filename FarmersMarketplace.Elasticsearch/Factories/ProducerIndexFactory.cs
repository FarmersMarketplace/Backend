using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Elasticsearch.Documents;
using Microsoft.EntityFrameworkCore;
using Nest;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.Factories
{
    public class ProducerIndexFactory : IIndexFactory
    {
        public CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor descriptor)
        {
            return descriptor
            .Map<ProducerDocument>(mappingDescriptor => mappingDescriptor.Dynamic(false)
                .Properties(props => props
                    .Number(n => n
                    .Name(order => order.Producer)
                    .Type(NumberType.Integer))
                    .Keyword(k => k
                        .Name(p => p.Id))
                    .Text(t => t
                        .Name(p => p.Name))
                    .Number(n => n
                        .Name(p => p.Longitude)
                        .Type(NumberType.Double))
                    .Number(n => n
                        .Name(p => p.Latitude)
                        .Type(NumberType.Double))
                    .Text(t => t
                        .Name(p => p.Region))
                    .Keyword(k => k
                        .Name(p => p.Subcategories))
                    .Keyword(k => k
                        .Name(p => p.ImageName)
                        .Index(false))
                    .Number(n => n
                        .Name(p => p.FeedbacksCount)
                        .Type(NumberType.Integer)
                        .Index(false))
                    .Number(n => n
                        .Name(p => p.Rating)
                        .Type(NumberType.Float)
                        .Index(false))));
        }

        public void CreateIndex(IElasticClient client)
        {
            string indexName = Indecies.Producers;

            if (!client.Indices.Exists(indexName).Exists)
            {
                var descriptor = new CreateIndexDescriptor(indexName);
                descriptor = ConfigureIndex(descriptor);
                client.Indices.Create(indexName, c => descriptor);
            }
        }

        public async Task LoadData(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper)
        {
            var deleteResponse = client.DeleteByQuery<ProducerDocument>(d => d
                .Index(Indecies.Producers)
                .Query(q => q.MatchAll()));

            if (!deleteResponse.IsValid)
            {
                string message = $"Producers documents was not deleted successfully from Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchProducersNotDeleted");
            }

            await LoadFarms(client, dbContext, mapper);
            await LoadSellers(client, dbContext, mapper);
        }

        private async Task LoadSellers(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper)
        {
            var sellers = await dbContext.Sellers.Include(s => s.Address)
                .Include(s => s.Feedbacks)
                .ToArrayAsync();

            var documents = new ProducerDocument[sellers.Length];

            for (int i = 0; i < sellers.Length; i++)
            {
                documents[i] = mapper.Map<ProducerDocument>(sellers[i]);
            }

            var bulkIndexResponse = client.Bulk(b => b
                .Index(Indecies.Producers)
                .IndexMany(documents)
                .Pretty(true));

            if (!bulkIndexResponse.IsValid)
            {
                string message = $"Sellers documents was not uploaded successfully to Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchProducersNotUpoaded");
            }
        }

        private async Task LoadFarms(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper)
        {
            var farms = await dbContext.Farms.Include(f => f.Address)
                .Include(f => f.Feedbacks)
                .ToArrayAsync();

            var documents = new ProducerDocument[farms.Length];

            for (int i = 0; i < farms.Length; i++)
            {
                documents[i] = mapper.Map<ProducerDocument>(farms[i]);
            }

            var bulkIndexResponse = client.Bulk(b => b
                .Index(Indecies.Producers)
                .IndexMany(documents)
                .Pretty(true));

            if (!bulkIndexResponse.IsValid)
            {
                string message = $"Farms documents was not uploaded successfully to Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchProducersNotUpoaded");
            }
        }
    }
}
