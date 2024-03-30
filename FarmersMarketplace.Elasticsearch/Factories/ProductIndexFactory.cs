using FarmersMarketplace.Domain;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;

namespace FarmersMarketplace.Elasticsearch.Factories
{
    public class ProductIndexFactory : IIndexFactory
    {
        public void CreateIndex(IElasticClient client)
        {
            string indexName = Indecies.Products;

            if (!client.Indices.Exists(indexName).Exists)
            {
                var descriptor = new CreateIndexDescriptor(indexName);
                descriptor = ConfigureIndex(descriptor);
                client.Indices.Create(indexName, c => descriptor);
            }
        }

        public CreateIndexDescriptor ConfigureIndex(CreateIndexDescriptor descriptor)
        {
            return descriptor
                .Map<ProductDocument>(mappingDescriptor => mappingDescriptor.Dynamic(false)
                    .Properties(props => props
                        .Keyword(k => k
                            .Name(product => product.Id))
                        .Keyword(k => k
                            .Name(product => product.ProducerId))
                        .Keyword(k => k
                            .Name(product => product.SubcategoryId))
                        .Text(t => t
                            .Name(product => product.Name))
                        .Text(t => t
                            .Name(product => product.ArticleNumber))
                        .Text(t => t
                            .Name(product => product.CategoryName))
                        .Text(t => t
                            .Name(product => product.SubcategoryName))
                        .Number(t => t
                            .Name(product => product.Count))
                        .Text(t => t
                            .Name(product => product.UnitOfMeasurement))
                        .Date(t => t
                            .Name(product => product.CreationDate))
                        .Number(t => t
                            .Name(product => product.PricePerOne))
                        .Number(t => t
                            .Name(product => product.Status))
                        .Date(t => t
                            .Name(product => product.ExpirationDate))
                        .Text(t => t
                            .Name(product => product.ImageName))
                        .Text(t => t
                            .Name(product => product.ProducerImageName))
                        .Number(t => t
                            .Name(product => product.Rating))
                        .Number(t => t
                            .Name(product => product.FeedbacksCount))));
        }
    }

}
