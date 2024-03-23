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
            MapProductToProductVm();
            MapProductToProductLookupVm();
        }

        private void MapProductToProductVm()
        {
            CreateMap<Product, ProductVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(product => product.Id))
                .ForMember(vm => vm.Name, opt => opt.MapFrom(product => product.Name))
                .ForMember(vm => vm.ArticleNumber, opt => opt.MapFrom(product => product.ArticleNumber))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(product => product.Description))
                .ForMember(vm => vm.CategoryId, opt => opt.MapFrom(product => product.CategoryId))
                .ForMember(vm => vm.SubcategoryId, opt => opt.MapFrom(product => product.SubcategoryId))
                .ForMember(vm => vm.Category, opt => opt.MapFrom(product => product.Category.Name))
                .ForMember(vm => vm.Subcategory, opt => opt.MapFrom(product => product.Subcategory.Name))
                .ForMember(vm => vm.Producer, opt => opt.MapFrom(product => product.Producer))
                .ForMember(vm => vm.ProducerName, opt => opt.MapFrom(product => ""))
                .ForMember(vm => vm.ProducerId, opt => opt.MapFrom(product => product.ProducerId))
                .ForMember(vm => vm.PackagingType, opt => opt.MapFrom(product => product.PackagingType))
                .ForMember(vm => vm.UnitOfMeasurement, opt => opt.MapFrom(product => product.UnitOfMeasurement))
                .ForMember(vm => vm.PricePerOne, opt => opt.MapFrom(product => product.PricePerOne))
                .ForMember(vm => vm.MinPurchaseQuantity, opt => opt.MapFrom(product => product.MinPurchaseQuantity))
                .ForMember(vm => vm.Count, opt => opt.MapFrom(product => product.Count))
                .ForMember(vm => vm.ExpirationDate, opt => opt.MapFrom(product => product.ExpirationDate))
                .ForMember(vm => vm.CreationDate, opt => opt.MapFrom(product => product.CreationDate))
                .ForMember(vm => vm.ImagesNames, opt => opt.MapFrom(product => product.ImagesNames))
                .ForMember(vm => vm.DocumentsNames, opt => opt.MapFrom(product => product.DocumentsNames));
        }

        private void MapCreateProductDtoToProduct()
        {
            CreateMap<CreateProductDto, Product>()
                .ForMember(product => product.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(product => product.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(product => product.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(product => product.CategoryId, opt => opt.MapFrom(dto => dto.CategoryId))
                .ForMember(product => product.SubcategoryId, opt => opt.MapFrom(dto => dto.SubcategoryId))
                .ForMember(product => product.Producer, opt => opt.MapFrom(dto => dto.Producer))
                .ForMember(product => product.ProducerId, opt => opt.MapFrom(dto => dto.ProducerId))
                .ForMember(product => product.PackagingType, opt => opt.MapFrom(dto => dto.PackagingType))
                .ForMember(product => product.UnitOfMeasurement, opt => opt.MapFrom(dto => dto.UnitOfMeasurement))
                .ForMember(product => product.PricePerOne, opt => opt.MapFrom(dto => dto.PricePerOne))
                .ForMember(product => product.MinPurchaseQuantity, opt => opt.MapFrom(dto => dto.MinPurchaseQuantity))
                .ForMember(product => product.Count, opt => opt.MapFrom(dto => dto.Count))
                .ForMember(product => product.Status, opt => opt.MapFrom(dto => dto.Status))
                .ForMember(product => product.CreationDate, opt => opt.MapFrom(dto => dto.CreationDate))
                .ForMember(product => product.ExpirationDate, opt => opt.MapFrom(dto => dto.ExpirationDate));
        }

        private void MapProductToProductLookupVm()
        {
            CreateMap<Product, ProductLookupVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(product => product.Id))
                .ForMember(vm => vm.Name, opt => opt.MapFrom(product => product.Name))
                .ForMember(vm => vm.ArticleNumber, opt => opt.MapFrom(product => product.ArticleNumber))
                .ForMember(vm => vm.Category, opt => opt.MapFrom(product => product.Category.Name))
                .ForMember(vm => vm.Subcategory, opt => opt.MapFrom(product => product.Subcategory.Name))
                .ForMember(vm => vm.Rest, opt => opt.MapFrom(product => product.Count))
                .ForMember(vm => vm.UnitOfMeasurement, opt => opt.MapFrom(product => product.UnitOfMeasurement))
                .ForMember(vm => vm.PricePerOne, opt => opt.MapFrom(product => product.PricePerOne))
                .ForMember(vm => vm.CreationDate, opt => opt.MapFrom(dto => dto.CreationDate))
                .ForMember(vm => vm.Status, opt => opt.MapFrom(dto => dto.Status));
        }
    }

}
