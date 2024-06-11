using AutoMapper;
using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Payment;
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
            var getResponse = await Client.GetAsync<ProducerDocument>(obj.Id);
            bool hasOnlinePayment = obj.PaymentTypes.Count > 0 ? obj.PaymentTypes.Contains(PaymentType.Online) : false;
            string imageName = obj.ImagesNames.Count > 0 ? obj.ImagesNames[0] : "";

            if (getResponse.Source.Region != obj.Address.Region ||
                getResponse.Source.Name != obj.Name ||
                getResponse.Source.HasOnlinePayment != hasOnlinePayment ||
                getResponse.Source.ImageName != imageName)
            {
                await UpdateProducts(obj);
            }

            var document = Mapper.Map<ProducerDocument>(obj);
            await Client.UpdateAsync<ProducerDocument, object>(obj.Id, u => u
                .Doc(document)
                .Index(Indecies.Producers));
        }

        private async Task UpdateProducts(Seller obj)
        {
            bool hasOnlinePayment = obj.PaymentTypes.Count > 0 ? obj.PaymentTypes.Contains(PaymentType.Online) : false;
            string imageName = obj.ImagesNames.Count > 0 ? obj.ImagesNames[0] : "";

            var searchResponse = await Client.SearchAsync<ProductDocument>(s => s
                .Index(Indecies.Products)
                .Query(q => q
                    .Bool(b => b
                        .Must(
                            m => m.Term(t => t.Producer, Producer.Seller),
                            m => m.Term(t => t.ProducerId, obj.Id)))));

            if (searchResponse.Documents.Any())
            {
                foreach (var product in searchResponse.Documents)
                {
                    product.Region = obj.Address.Region;
                    product.ProducerName = obj.Name;
                    product.HasOnlinePayment = hasOnlinePayment;
                    product.ImageName = imageName;

                    await Client.UpdateAsync<ProductDocument, object>(product.Id, u => u
                        .Doc(product)
                        .Index(Indecies.Products));
                }
            }
        }
    }
}
