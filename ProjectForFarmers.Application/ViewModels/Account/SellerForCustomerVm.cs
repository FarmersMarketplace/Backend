using FarmersMarketplace.Application.ViewModels.Feedback;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.ViewModels.Account
{
    public class SellerForCustomerVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AdditionalPhone { get; set; }
        public AddressVm Address { get; set; }
        public ScheduleVm Schedule { get; set; }
        public string FirstSocialPageUrl { get; set; }
        public string SecondSocialPageUrl { get; set; }
        public List<string> ImagesNames { get; set; }
        public List<ReceivingMethod> ReceivingMethods { get; set; }
        public bool HasDocuments { get; set; }
        public FeedbackCollectionVm Feedbacks { get; set; }
    }
}
