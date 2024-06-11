using FarmersMarketplace.Application.Exceptions;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Payment;
using FarmersMarketplace.Elasticsearch.Documents;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace FarmersMarketplace.Elasticsearch.Synchronizers
{
    public class ProductSynchronizer : ISearchSynchronizer<Product>
    {
        private readonly IElasticClient Client;
        private readonly IApplicationDbContext DbContext;

        public ProductSynchronizer(IElasticClient client, IApplicationDbContext dbContext)
        {
            Client = client;
            DbContext = dbContext;
        }

        public async Task Create(Product obj)
        {
            var document = new ProductDocument
            {
                Id = obj.Id,
                Name = obj.Name,
                ArticleNumber = obj.ArticleNumber,
                SubcategoryId = obj.SubcategoryId,
                SubcategoryName = obj.Subcategory?.Name,
                CategoryName = obj.Category?.Name,
                Status = obj.Status,
                Producer = obj.Producer,
                ProducerId = obj.ProducerId,
                PackagingType = obj.PackagingType,
                UnitOfMeasurement = obj.UnitOfMeasurement,
                PricePerOne = obj.PricePerOne,
                Count = obj.Count,
                ExpirationDate = obj.CreationDate.AddDays(obj.ExpirationDays),
                CreationDate = obj.CreationDate,
                ImageName = obj.ImagesNames?.FirstOrDefault() ?? "",
                ReceivingMethods = obj.ReceivingMethods,
                Rating = obj.Rating,
                FeedbacksCount = (uint)(obj.Feedbacks?.Count ?? 0),
            };

            if (obj.Producer == Producer.Farm)
            {
                await SetFarmData(document);
            }
            else if (obj.Producer == Producer.Seller)
            {  
                await SetSellerData(document);
            }
            else
            {
                throw new NotImplementedException("Uncorrect type of producer.");
            }

            await Client.IndexDocumentAsync(document);
        }

        private async Task SetSellerData(ProductDocument document)
        {
            var seller = await DbContext.Sellers.Include(c => c.Address)
                .FirstOrDefaultAsync(s => s.Id == document.ProducerId);

            if (seller == null)
            {
                string message = $"Seller with Id {document.ProducerId} was not found.";
                throw new NotFoundException(message, "RetrievingDataError");
            }

            document.Region = seller.Address.Region;
            document.ProducerImageName = seller.ImagesNames?.FirstOrDefault() ?? "";
            document.ProducerName = seller.Name;
            document.HasOnlinePayment = seller.PaymentTypes.Contains(PaymentType.Online);
        }

        private async Task SetFarmData(ProductDocument document)
        {
            var farm = await DbContext.Farms.Include(c => c.Address)
                .FirstOrDefaultAsync(s => s.Id == document.ProducerId);

            if (farm == null)
            {
                string message = $"Farm with Id {document.ProducerId} was not found.";
                throw new NotFoundException(message, "RetrievingDataError");
            }

            document.Region = farm.Address.Region;
            document.ProducerImageName = farm.ImagesNames?.FirstOrDefault() ?? "";
            document.ProducerName = farm.Name;
            document.HasOnlinePayment = farm.PaymentTypes.Contains(PaymentType.Online);
        }

        public async Task Delete(Guid id)
        {
            await Client.DeleteAsync<ProductDocument>(id);
        }

        public async Task Update(Product obj)
        {
            await Client.UpdateAsync<ProductDocument, object>(obj.Id, u => u
                .Doc(new ProductDocument
                {
                    Name = obj.Name,
                    ArticleNumber = obj.ArticleNumber,
                    SubcategoryId = obj.SubcategoryId,
                    SubcategoryName = obj.Subcategory?.Name,
                    CategoryName = obj.Category?.Name,
                    Status = obj.Status,
                    Producer = obj.Producer,
                    ProducerId = obj.ProducerId,
                    PackagingType = obj.PackagingType,
                    UnitOfMeasurement = obj.UnitOfMeasurement,
                    PricePerOne = obj.PricePerOne,
                    Count = obj.Count,
                    ExpirationDate = obj.CreationDate.AddDays(obj.ExpirationDays),
                    CreationDate = obj.CreationDate,
                    ImageName = obj.ImagesNames?.FirstOrDefault() ?? "",
                    ReceivingMethods = obj.ReceivingMethods,
                    Rating = obj.Rating,
                    FeedbacksCount = (uint)(obj.Feedbacks?.Count ?? 0),
                })
                .Index(Indecies.Products));
        }
    }
}
