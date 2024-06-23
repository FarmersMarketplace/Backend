using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Feedback;
using FarmersMarketplace.Domain.Feedbacks;
using FarmersMarketplace.Elasticsearch.Documents;

namespace FarmersMarketplace.Elasticsearch.Mappings
{
    public class FeedbackSearchProfile : Profile
    {
        public FeedbackSearchProfile()
        {
            MapFeedbackToFeedbackDocument();
            MapFeedbackDocumentToFeedbackForEntityVm();
            MapFeedbackDocumentToFeedbackForCustomerVm();

        }

        private void MapFeedbackToFeedbackDocument()
        {
            CreateMap<Feedback, FeedbackDocument>()
                .ForMember(document => document.CustomerId, opt => opt.MapFrom(feedback => feedback.CustomerId))
                .ForMember(document => document.CustomerName, opt => opt.MapFrom(feedback => feedback.Customer.Name + " " + feedback.Customer.Surname))
                .ForMember(document => document.CustomerId, opt => opt.MapFrom(feedback => feedback.Customer.AvatarName));
        }


        private void MapFeedbackDocumentToFeedbackForEntityVm()
        {
            CreateMap<FeedbackDocument, FeedbackForEntityVm>();
        }

        private void MapFeedbackDocumentToFeedbackForCustomerVm()
        {
            CreateMap<FeedbackDocument, FeedbackForCustomerVm>();
        }

    }
}
