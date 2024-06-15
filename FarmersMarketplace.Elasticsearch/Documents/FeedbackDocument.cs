using FarmersMarketplace.Domain.Accounts;

namespace FarmersMarketplace.Elasticsearch.Documents
{
    public class FeedbackDocument
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
        public Guid ReviewedEntityId { get; set; }
        public Customer Customer { get; set; }
    }
}
