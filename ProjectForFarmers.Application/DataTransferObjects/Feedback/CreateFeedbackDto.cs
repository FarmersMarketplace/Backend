using FarmersMarketplace.Domain.Feedbacks;

namespace FarmersMarketplace.Application.DataTransferObjects.Feedback
{
    public class CreateFeedbackDto
    {
        public Guid CustomerId { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public FeedbackType Type { get; set; }
        public Guid ReviewedEntityId { get; set; }
    }
}
