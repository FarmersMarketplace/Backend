using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Producers;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Payment;
using FarmersMarketplace.Elasticsearch.Documents;

namespace FarmersMarketplace.Elasticsearch.Mappings
{
    public class ProducerSearchProfile : Profile
    {
        public ProducerSearchProfile()
        {
            MapProducerDocmentToProducerLookupVm();
            MapSellerToProducerDocument();
            MapFarmToProducerDocument();
        }

        private void MapFarmToProducerDocument()
        {
            CreateMap<Farm, ProducerDocument>()
                .ForMember(document => document.Name, opt => opt.MapFrom(farm => Producer.Farm))
                .ForMember(document => document.Name, opt => opt.MapFrom(farm => farm.Name))
                .ForMember(document => document.Longitude, opt => opt.MapFrom(farm => farm.Address.Longitude))
                .ForMember(document => document.Latitude, opt => opt.MapFrom(farm => farm.Address.Latitude))
                .ForMember(document => document.Region, opt => opt.MapFrom(farm => farm.Address.Region))
                .ForMember(document => document.Subcategories, opt => opt.MapFrom(farm => new HashSet<Guid>(farm.Subcategories)))
                .ForMember(document => document.ImageName, opt => opt.MapFrom(farm => farm.ImagesNames.Count > 0 ? farm.ImagesNames[0] : ""))
                .ForMember(document => document.FeedbacksCount, opt => opt.MapFrom(farm => farm.Feedbacks.Count))
                .ForMember(document => document.Rating, opt => opt.MapFrom(farm => farm.Rating))
                .ForMember(document => document.HasOnlinePayment, opt => opt.MapFrom(farm => farm.PaymentTypes.Count > 0 ? farm.PaymentTypes.Contains(PaymentType.Online) : false));
        }

        private void MapSellerToProducerDocument()
        {
            CreateMap<Seller, ProducerDocument>()
                .ForMember(document => document.Name, opt => opt.MapFrom(farm => Producer.Seller))
                .ForMember(document => document.Name, opt => opt.MapFrom(seller => seller.Name + " " + seller.Surname))
                .ForMember(document => document.Longitude, opt => opt.MapFrom(seller => seller.Address.Longitude))
                .ForMember(document => document.Latitude, opt => opt.MapFrom(seller => seller.Address.Latitude))
                .ForMember(document => document.Region, opt => opt.MapFrom(seller => seller.Address.Region))
                .ForMember(document => document.Subcategories, opt => opt.MapFrom(seller => new HashSet<Guid>(seller.Subcategories)))
                .ForMember(document => document.ImageName, opt => opt.MapFrom(seller => seller.ImagesNames.Count > 0 ? seller.ImagesNames[0] : ""))
                .ForMember(document => document.FeedbacksCount, opt => opt.MapFrom(seller => seller.Feedbacks.Count))
                .ForMember(document => document.Rating, opt => opt.MapFrom(seller => seller.Rating))
                .ForMember(document => document.HasOnlinePayment, opt => opt.MapFrom(seller => seller.PaymentTypes.Count > 0 ? seller.PaymentTypes.Contains(PaymentType.Online) : false));
        }

        private void MapProducerDocmentToProducerLookupVm()
        {
            CreateMap<ProducerDocument, ProducerLookupVm>();
        }
    }
}
