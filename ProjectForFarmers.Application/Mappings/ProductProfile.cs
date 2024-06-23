using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            MapCreateProductDtoToProduct();
            MapProductToProductForProducerVm();
            MapProductToProductForCustomerVm();
            MapProductToProducerProductLookupVm();
        }

        private void MapProductToProductForCustomerVm()
        {
            CreateMap<Product, ProductForCustomerVm>()
                .ForMember(vm => vm.Feedbacks, opt => opt.MapFrom(product => product.Feedbacks));
        }

        private void MapProductToProductForProducerVm()
        {
            CreateMap<Product, ProductForProducerVm>()
                .ForMember(vm => vm.Category, opt => opt.MapFrom(product => product.Category.Name))
                .ForMember(vm => vm.Subcategory, opt => opt.MapFrom(product => product.Subcategory.Name))
                .ForMember(vm => vm.ProducerName, opt => opt.MapFrom(product => ""));
        }

        private void MapCreateProductDtoToProduct()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(product => product.Id, opt => opt.MapFrom(dto => Guid.NewGuid()));
        }

        private void MapProductToProducerProductLookupVm()
        {
            CreateMap<Product, ProducerProductLookupVm>()
                .ForMember(vm => vm.Category, opt => opt.MapFrom(product => product.Category.Name))
                .ForMember(vm => vm.Subcategory, opt => opt.MapFrom(product => product.Subcategory.Name))
                .ForMember(vm => vm.Rest, opt => opt.MapFrom(product => product.Count));
        }
    }

}
