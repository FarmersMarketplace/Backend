using FarmersMarketplace.Domain.Feedbacks;

namespace FarmersMarketplace.Application.ViewModels.Feedback
{
    public class FeedbackForCustomerVm
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
        public Guid ReviewedEntityId { get; set; }
        public FeedbackType ReviewedEntity { get; set; }
        public string ReviewedEntityName { get; set; }
        public string ReviewedEntityImage { get; set; }
    }
}
