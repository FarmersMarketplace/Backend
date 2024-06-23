using FarmersMarketplace.Domain.Feedbacks;

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
        public FeedbackType ReviewedEntity { get; set; }
        public string ReviewedEntityName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public string ReviewedEntityImage { get; set; }
    }
}
