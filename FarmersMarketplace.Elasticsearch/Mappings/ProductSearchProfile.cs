using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Elasticsearch.Documents;

namespace FarmersMarketplace.Elasticsearch.Mappings
{
    public class ProductSearchProfile : Profile
    {
        public ProductSearchProfile()
        {
            MapProductToProductDocument();
            MapProductDocumentToCustomerProductLookupVm();
            MapProductDocumentToProducerProductLookupVm();
        }

        private void MapProductDocumentToProducerProductLookupVm()
        {
            CreateMap<ProductDocument, ProducerProductLookupVm>();
        }

        private void MapProductDocumentToCustomerProductLookupVm()
        {
                CreateMap<ProductDocument, ProducerProductLookupVm>()
                    .ForMember(vm => vm.Category, opt => opt.MapFrom(document => document.CategoryName))
                    .ForMember(vm => vm.Subcategory, opt => opt.MapFrom(document => document.SubcategoryName))
                    .ForMember(vm => vm.Rest, opt => opt.MapFrom(document => document.Count));
        }

        private void MapProductToProductDocument()
        {
            CreateMap<Product, ProductDocument>()
                .ForMember(document => document.SubcategoryName, opt => opt.MapFrom(product => product.Subcategory.Name))
                .ForMember(document => document.CategoryName, opt => opt.MapFrom(product => product.Category.Name))
                .ForMember(document => document.ExpirationDate, opt => opt
                    .MapFrom(product => product.CreationDate.AddDays(product.ExpirationDays)))
                .ForMember(document => document.ImageName, opt => opt.MapFrom(product =>
                    product.ImagesNames != null && product.ImagesNames.Count > 0
                    ? product.ImagesNames[0]
                    : ""))
                .ForMember(document => document.ProducerImageName, opt => opt.MapFrom(product => ""))
                .ForMember(document => document.ProducerName, opt => opt.MapFrom(product => ""))
                .ForMember(document => document.FeedbacksCount, opt => opt.MapFrom(product => product.Feedbacks.Count));
        }
    }

}
