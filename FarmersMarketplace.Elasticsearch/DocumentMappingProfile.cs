using AutoMapper;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Elasticsearch.Documents;
using System.Reflection.Metadata;

namespace FarmersMarketplace.Elasticsearch
{
    public class DocumentMappingProfile : Profile
    {
        public DocumentMappingProfile()
        {
            MapProductToProductDocument();
        }

        private void MapProductToProductDocument()
        {
            CreateMap<Product, ProductDocument>()
                .ForMember(document => document.SubcategoryName, opt => opt.MapFrom(product => product.Subcategory.Name))
                .ForMember(document => document.CategoryName, opt => opt.MapFrom(product => product.Category.Name))
                .ForMember(document => document.ExpirationDate, opt => opt
                    .MapFrom(product => product.CreationDate.AddDays(product.ExpirationDays)))
                .ForMember(document => document.ImageName, opt => opt.MapFrom(product =>
                    (product.ImagesNames != null && product.ImagesNames.Count > 0)
                    ? product.ImagesNames[0]
                    : ""))
                .ForMember(document => document.ProducerImageName, opt => opt.MapFrom(product => ""))
                .ForMember(document => document.ProducerName, opt => opt.MapFrom(product => ""))
                .ForMember(document => document.FeedbacksCount, opt => opt.MapFrom(product => product.Feedbacks.Count));
        }
    }

}
