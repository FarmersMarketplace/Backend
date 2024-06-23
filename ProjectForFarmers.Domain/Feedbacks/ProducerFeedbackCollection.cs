namespace FarmersMarketplace.Domain.Feedbacks
{
    public class ProducerFeedbackCollection : FeedbackCollection
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
    }
}
