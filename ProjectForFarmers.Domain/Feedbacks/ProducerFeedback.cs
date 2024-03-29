namespace FarmersMarketplace.Domain.Feedbacks
{
    public class ProducerFeedback : Feedback
    {
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
    }

}
