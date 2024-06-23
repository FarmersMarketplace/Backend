namespace FarmersMarketplace.Application.DataTransferObjects.Feedback
{
    public class GetCustomerFeedbackListDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Guid CustomerId { get; set; }
    }
}
