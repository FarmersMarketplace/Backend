namespace FarmersMarketplace.Application.ViewModels.Feedback
{
    public class FeedbackVm
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
