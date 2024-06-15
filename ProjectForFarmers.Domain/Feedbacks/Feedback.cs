namespace FarmersMarketplace.Domain.Feedbacks
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
        public Guid CollectionId { get; set; }
        public FeedbackType ReviewedEntity { get; set; }
    }

}
