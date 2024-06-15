using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Feedbacks;
using FarmersMarketplace.Elasticsearch.Documents;
using Microsoft.EntityFrameworkCore;
using Nest;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;


namespace FarmersMarketplace.Elasticsearch.Factories
{
    public class FeedbackIndexFactory : IIndexFactory
    {
        public CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor descriptor)
        {
            return descriptor
                .Map<FeedbackDocument>(mappingDescriptor => mappingDescriptor.Dynamic(false)
                    .Properties(props => props
                        .Keyword(k => k
                            .Name(feedback => feedback.Id))
                        .Keyword(k => k
                            .Name(feedback => feedback.CustomerId))
                        .Text(t => t
                            .Name(feedback => feedback.Comment))
                        .Number(n => n
                            .Name(feedback => feedback.Rating)
                            .Type(NumberType.Float))
                        .Date(d => d
                            .Name(feedback => feedback.Date))
                        .Keyword(k => k
                            .Name(feedback => feedback.ReviewedEntityId))
                        .Number(n => n
                            .Name(feedback => feedback.ReviewedEntity)
                            .Type(NumberType.Integer))
                        .Text(t => t
                            .Name(feedback => feedback.ReviewedEntityName))
                        .Text(t => t
                            .Name(feedback => feedback.CustomerName))
                        .Keyword(k => k
                            .Name(feedback => feedback.CustomerImage)
                            .Index(false))
                        .Keyword(k => k
                            .Name(feedback => feedback.ReviewedEntityImage)
                            .Index(false))));
        }

        public void CreateIndex(IElasticClient client)
        {
            string indexName = Indecies.Feedbacks;

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
                throw new ApplicationException(message, "ElasticsearchFeedbacksNotDeleted");
            }

            var farms = await dbContext.Farms.Include(f => f.Feedbacks).ToListAsync();
            var sellers = await dbContext.Sellers.Include(s => s.Feedbacks).ToListAsync();
            var products = await dbContext.Products.Include(p => p.Feedbacks).ToListAsync();

            await LoadSellersFeedbacks(sellers, client, mapper);
            await LoadFarmsFeedbacks(farms, client, mapper);
            await LoadProductsFeedbacks(products, client, mapper);
        }

        private async Task LoadProductsFeedbacks(List<Product> products, IElasticClient client, IMapper mapper)
        {
            var documents = new List<FeedbackDocument>();

            for (int i = 0; i < products.Count; i++)
            {
                for (int j = 0; j < products[i].Feedbacks.Feedbacks.Count; j++)
                {
                    var document = mapper.Map<FeedbackDocument>(products[i].Feedbacks.Feedbacks[j]);
                    document.ReviewedEntityId = products[i].Id;
                    document.ReviewedEntityName = products[i].Name;
                    document.ReviewedEntity = FeedbackType.Product;
                    document.ReviewedEntityImage = products[i].ImagesNames.Count > 0 ? products[i].ImagesNames[0] : "";

                    documents.Add(document);
                }
            }

            var bulkIndexResponse = client.Bulk(b => b
                .Index(Indecies.Feedbacks)
                .IndexMany(documents)
                .Pretty(true));

            if (!bulkIndexResponse.IsValid)
            {
                string message = $"Feedbacks documents was not uploaded successfully to Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchFeedbacksNotUpoaded");
            }
        }

        private async Task LoadFarmsFeedbacks(List<Farm> farms, IElasticClient client, IMapper mapper)
        {
            var documents = new List<FeedbackDocument>();

            for (int i = 0; i < farms.Count; i++)
            {
                for (int j = 0; j < farms[i].Feedbacks.Feedbacks.Count; j++)
                {
                    var document = mapper.Map<FeedbackDocument>(farms[i].Feedbacks.Feedbacks[j]);
                    document.ReviewedEntityId = farms[i].Id;
                    document.ReviewedEntityName = farms[i].Name;
                    document.ReviewedEntity = FeedbackType.Farm;
                    document.ReviewedEntityImage = farms[i].ImagesNames.Count > 0 ? farms[i].ImagesNames[0] : "";

                    documents.Add(document);
                }
            }

            var bulkIndexResponse = client.Bulk(b => b
                .Index(Indecies.Feedbacks)
                .IndexMany(documents)
                .Pretty(true));

            if (!bulkIndexResponse.IsValid)
            {
                string message = $"Feedbacks documents was not uploaded successfully to Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchFeedbacksNotUpoaded");
            }
        }

        private async Task LoadSellersFeedbacks(List<Seller> sellers, IElasticClient client, IMapper mapper)
        {
            var documents = new List<FeedbackDocument>();

            for (int i = 0; i < sellers.Count; i++)
            {
                for (int j = 0; j < sellers[i].Feedbacks.Feedbacks.Count; j++)
                {
                    var document = mapper.Map<FeedbackDocument>(sellers[i].Feedbacks.Feedbacks[j]);
                    document.ReviewedEntityId = sellers[i].Id;
                    document.ReviewedEntityName = sellers[i].Name + sellers[i].Surname;
                    document.ReviewedEntity = FeedbackType.Seller;
                    document.ReviewedEntityImage = sellers[i].ImagesNames.Count > 0 ? sellers[i].ImagesNames[0] : "";

                    documents.Add(document);
                }
            }

            var bulkIndexResponse = client.Bulk(b => b
                .Index(Indecies.Feedbacks)
                .IndexMany(documents)
                .Pretty(true));

            if (!bulkIndexResponse.IsValid)
            {
                string message = $"Feedbacks documents was not uploaded successfully to Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchFeedbacksNotUpoaded");
            }
        }
    }
}
