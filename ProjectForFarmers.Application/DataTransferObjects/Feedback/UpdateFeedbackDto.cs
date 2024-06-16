namespace FarmersMarketplace.Application.DataTransferObjects.Feedback
{
    public class UpdateFeedbackDto
    {
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
    }
}
