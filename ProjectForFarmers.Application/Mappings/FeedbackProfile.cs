using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Feedback;
using FarmersMarketplace.Domain.Feedbacks;

namespace FarmersMarketplace.Application.Mappings
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            MapProducerFeedbackCollectionToFeedbackCollectionVm();
            MapProductFeedbackCollectionToFeedbackCollectionVm();
            MapFeedbackToFeedbackVm();
        }

        private void MapFeedbackToFeedbackVm()
        {
            CreateMap<Feedback, FeedbackVm>()
                .ForMember(vm => vm.CustomerName, opt => opt.MapFrom(feedback => feedback.Customer.Name + " " + feedback.Customer.Surname));
        }

        private void MapProductFeedbackCollectionToFeedbackCollectionVm()
        {
            CreateMap<ProductFeedbackCollection, FeedbackCollectionVm>();
        }

        private void MapProducerFeedbackCollectionToFeedbackCollectionVm()
        {
            CreateMap<ProducerFeedbackCollection, FeedbackCollectionVm>();
        }
    }
}
