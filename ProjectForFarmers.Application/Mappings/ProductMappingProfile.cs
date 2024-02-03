using AutoMapper;
using ProjectForFarmers.Application.DataTransferObjects.Product;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            MapProductDtoToProduct();
        }

        private void MapProductDtoToProduct()
        {
            CreateMap<ProductDto, Product>()
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
                .ForMember(product => product.ReceivingTypes, opt => opt.MapFrom(dto => dto.ReceivingTypes))
                .ForMember(product => product.ExpirationDate, opt => opt.MapFrom(dto => dto.ExpirationDate));
        }
    }

}
