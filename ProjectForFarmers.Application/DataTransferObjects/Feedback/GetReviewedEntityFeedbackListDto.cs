using FarmersMarketplace.Domain.Feedbacks;

namespace FarmersMarketplace.Application.DataTransferObjects.Feedback
{
    public class GetReviewedEntityFeedbackListDto
    {
        public Guid ReviewedEntityId { get; set; }
        public FeedbackType Type { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
