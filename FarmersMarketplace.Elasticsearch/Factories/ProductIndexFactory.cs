using AutoMapper;
using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Helpers;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Payment;
using FarmersMarketplace.Elasticsearch.Documents;
using Microsoft.EntityFrameworkCore;
using Nest;
using ApplicationException = FarmersMarketplace.Application.Exceptions.ApplicationException;

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
                            .Name(product => product.ProducerId)
                            .Index(false))
                        .Keyword(k => k
                            .Name(product => product.SubcategoryId)
                            .Index(false))
                        .Text(t => t
                            .Name(product => product.Name))
                        .Text(t => t
                            .Name(product => product.ArticleNumber))
                        .Text(t => t
                            .Name(product => product.CategoryName)
                            .Index(false))
                        .Text(t => t
                            .Name(product => product.SubcategoryName)
                            .Index(false))
                        .Number(t => t
                            .Name(product => product.Count)
                            .Index(false))
                        .Keyword(t => t
                            .Name(product => product.UnitOfMeasurement)
                            .Index(false))
                        .Date(t => t
                            .Name(product => product.CreationDate)
                            .Index(false))
                        .Number(t => t
                            .Name(product => product.PricePerOne)
                            .Index(false))
                        .Number(t => t
                            .Name(product => product.Status)
                            .Index(false))
                        .Date(t => t
                            .Name(product => product.ExpirationDate)
                            .Index(false))
                        .Keyword(t => t
                            .Name(product => product.ImageName)
                            .Index(false))
                        .Text(t => t
                            .Name(product => product.ProducerName)
                            .Index(false))
                        .Keyword(t => t
                            .Name(product => product.Region)
                            .Index(false))
                        .Keyword(t => t
                            .Name(product => product.ProducerImageName)
                            .Index(false))
                        .Number(t => t
                            .Name(product => product.Rating)
                            .Index(false))
                        .Number(t => t
                            .Name(product => product.FeedbacksCount)
                            .Index(false))
                        .Keyword(t => t
                            .Name(product => product.Producer)
                            .Index(false))
                        .Boolean(t => t
                            .Name(product => product.HasOnlinePayment)
                            .Index(false))
                        .Number(n => n
                            .Name(p => p.ReceivingMethods)
                            .Index(false)
                            .Type(NumberType.Integer))));
        }

        public async Task LoadData(IElasticClient client, IApplicationDbContext dbContext, IMapper mapper)
        {
            var deleteResponse = client.DeleteByQuery<ProductDocument>(d => d
                .Index(Indecies.Products)
                .Query(q => q.MatchAll()));

            if (!deleteResponse.IsValid)
            {
                string message = $"Products documents was not deleted successfully from Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchProductsNotDeleted");
            }

            var products = await dbContext.Products.Include(p => p.Subcategory)
                .Include(p => p.Category)
                .Include(p => p.Feedbacks)
                .ToArrayAsync();

            var documents = new ProductDocument[products.Length];

            for (int i = 0; i < products.Length; i++)
            {
                documents[i] = mapper.Map<ProductDocument>(products[i]);

                if (products[i].Producer == Producer.Farm)
                {
                    var farm = await dbContext.Farms.Include(f => f.Address)
                        .FirstOrDefaultAsync(f => f.Id == products[i].ProducerId);

                    if (farm == null)
                    {
                        string message = $"Farm with Id {products[i].ProducerId} was not found.";
                        throw new NotFoundException(message, "FarmNotFound");
                    }

                    documents[i].ProducerName = farm.Name;
                    documents[i].ProducerImageName =
                        (farm.ImagesNames != null && farm.ImagesNames.Count > 0)
                        ? farm.ImagesNames[0]
                        : "";

                    documents[i].HasOnlinePayment = farm.PaymentTypes != null && farm.PaymentTypes.Contains(PaymentType.Online);
                    documents[i].Region = farm.Address.Region;
                }
                else if (products[i].Producer == Producer.Seller)
                {
                    var seller = await dbContext.Sellers.Include(s => s.Address)
                        .FirstOrDefaultAsync(f => f.Id == products[i].ProducerId);

                    if (seller == null)
                    {
                        string message = $"Account with Id {products[i].ProducerId} was not found.";
                        throw new NotFoundException(message, "AccountNotFound");
                    }

                    documents[i].ProducerName = seller.Surname + " " + seller.Name;
                    documents[i].ProducerImageName =
                        (seller.ImagesNames != null && seller.ImagesNames.Count > 0)
                        ? seller.ImagesNames[0]
                        : "";

                    documents[i].HasOnlinePayment = seller.PaymentTypes != null && seller.PaymentTypes.Contains(PaymentType.Online);
                    documents[i].Region = seller.Address.Region;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

           var bulkIndexResponse = client.Bulk(b => b
                .Index(Indecies.Products)
                .IndexMany(documents)
                .Pretty(true));

            if (!bulkIndexResponse.IsValid)
            {
                string message = $"Products documents was not uploaded successfully to Elasticsearch.";
                throw new ApplicationException(message, "ElasticsearchProductsNotUpoaded");
            }
        }
    }

}
