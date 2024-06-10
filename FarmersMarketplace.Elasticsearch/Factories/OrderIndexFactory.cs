using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Elasticsearch.Documents;
using Microsoft.EntityFrameworkCore;
using Nest;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

namespace FarmersMarketplace.Elasticsearch.Factories
{
    public class OrderIndexFactory : IIndexFactory
    {
        public CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor descriptor)
        {
            return descriptor
        .Map<OrderDocument>(mappingDescriptor => mappingDescriptor.Dynamic(false)
            .Properties(props => props
                .Keyword(k => k
                    .Name(order => order.Id))
                .Text(t => t
                    .Name(order => order.Number))
                .Date(d => d
                    .Name(order => order.CreationDate))
                .Number(n => n
                    .Name(order => order.TotalPayment)
                    .Type(NumberType.Double))
                .Number(n => n
                    .Name(order => order.PaymentType)
                    .Type(NumberType.Integer))
                .Number(n => n
                    .Name(order => order.Status)
                    .Type(NumberType.Integer))
                .Text(t => t
                    .Name(order => order.CustomerName))
                .Text(t => t
                    .Name(order => order.CustomerSurname))
                .Keyword(k => k
                    .Name(order => order.CustomerPhone)
                    .Index(false))
                .Keyword(k => k
                    .Name(order => order.CustomerEmail)
                    .Index(false))
                .Number(n => n
                    .Name(order => order.Producer)
                    .Type(NumberType.Integer))
                .Keyword(k => k
                    .Name(order => order.ProducerId))
                .Keyword(k => k
                    .Name(order => order.CustomerId))));
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

        public async Task LoadData(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper)
        {
            var deleteResponse = client.DeleteByQuery<OrderDocument>(d => d
                .Index(Indecies.Orders)
                .Query(q => q.MatchAll()));

            if (!deleteResponse.IsValid)
            {
                string message = $"Orders documents was not deleted successfully from Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchOrdersNotDeleted");
            }

            var orders = await dbContext.Orders.Include(o => o.Customer)
                .ToArrayAsync();

            var documents = new OrderDocument[orders.Length];

            for (int i = 0; i < orders.Length; i++)
            {
                documents[i] = mapper.Map<OrderDocument>(orders[i]);
            }

            var bulkIndexResponse = client.Bulk(b => b
                .Index(Indecies.Orders)
                .IndexMany(documents)
                .Pretty(true));

            if (!bulkIndexResponse.IsValid)
            {
                string message = $"Orders documents was not uploaded successfully to Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchOrdersNotUpoaded");
            }
        }
    }
}
