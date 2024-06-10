using FarmersMarketplace.Domain.Orders;
using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Elasticsearch.Documents
{
    public class OrderDocument
    {
        public Guid Id { get; set; }
        public uint Number { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal TotalPayment { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderStatus Status { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
    }
}
