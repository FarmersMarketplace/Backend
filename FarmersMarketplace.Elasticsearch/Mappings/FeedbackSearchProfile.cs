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
            CreateMap<FeedbackDocument, FeedbackForEntityVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(document => document.Id))
                .ForMember(vm => vm.CustomerName, opt => opt.MapFrom(document => document.CustomerName))
                .ForMember(vm => vm.Comment, opt => opt.MapFrom(document => document.Comment))
                .ForMember(vm => vm.Rating, opt => opt.MapFrom(document => document.Rating))
                .ForMember(vm => vm.Date, opt => opt.MapFrom(document => document.Date))
                .ForMember(vm => vm.CustomerImage, opt => opt.MapFrom(document => document.CustomerImage));
        }

    }
}
