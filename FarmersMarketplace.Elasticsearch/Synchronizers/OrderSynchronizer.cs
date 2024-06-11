using FarmersMarketplace.Application.Interfaces;
using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Elasticsearch.Documents;
using Nest;

namespace FarmersMarketplace.Elasticsearch.Synchronizers
{
    public class OrderSynchronizer : ISearchSynchronizer<Order>
    {
        private readonly IElasticClient Client;

        public OrderSynchronizer(IElasticClient client)
        {
            Client = client;
        }

        public async Task Create(Order obj)
        {
            await Client.IndexDocumentAsync(new OrderDocument
            {
                Id = obj.Id,
                Number = obj.Number.ToString(),
                CreationDate = obj.CreationDate,
                TotalPayment = obj.TotalPayment,
                PaymentType = obj.PaymentType,
                Status = obj.Status,
                CustomerName = obj.CustomerName,
                CustomerSurname = obj.CustomerSurname,
                CustomerPhone = obj.CustomerPhone,
                CustomerEmail = obj.Customer.Email,
                Producer = obj.Producer,
                ProducerId = obj.ProducerId,
                CustomerId = obj.CustomerId
            });
        }

        public async Task Delete(Guid id)
        {
            await Client.DeleteAsync<OrderDocument>(id);
        }

        public async Task Update(Order obj)
        {
            await Client.UpdateAsync<OrderDocument, object>(obj.Id, u => u
                .Doc(new OrderDocument
                {
                    Number = obj.Number.ToString(),
                    CreationDate = obj.CreationDate,
                    TotalPayment = obj.TotalPayment,
                    PaymentType = obj.PaymentType,
                    Status = obj.Status,
                    CustomerName = obj.CustomerName,
                    CustomerSurname = obj.CustomerSurname,
                    CustomerPhone = obj.CustomerPhone,
                    CustomerEmail = obj.Customer.Email,
                    Producer = obj.Producer,
                    ProducerId = obj.ProducerId,
                    CustomerId = obj.CustomerId
                })
                .Index(Indecies.Orders));
        }
    }

}
