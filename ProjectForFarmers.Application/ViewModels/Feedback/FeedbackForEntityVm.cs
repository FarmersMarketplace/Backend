namespace FarmersMarketplace.Application.ViewModels.Feedback
{
    public class FeedbackForEntityVm
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerImage { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
