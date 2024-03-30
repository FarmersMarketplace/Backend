using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects.Product;
using FarmersMarketplace.Application.ViewModels.Product;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            MapCreateProductDtoToProduct();
            MapProductToProducerProductVm();
            MapProductToProducerProductLookupVm();
            MapProductToCustomerLookupVm();
        }

        private void MapProductToProducerProductVm()
        {
            CreateMap<Product, ProducerProductVm>()
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
