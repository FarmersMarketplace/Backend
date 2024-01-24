using AutoMapper;
using ProjectForFarmers.Application.DataTransferObjects.Auth;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class AddressDto : IMapWith<Address>
    {
        public string Region { get; set; }
        public string District { get; set; }
        public string Settlement { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string Note { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AddressDto, Address>()
                .ForMember(address => address.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(address => address.Region, opt => opt.MapFrom(dto => dto.Region))
                .ForMember(address => address.District, opt => opt.MapFrom(dto => dto.District))
                .ForMember(address => address.Settlement, opt => opt.MapFrom(dto => dto.Settlement))
                .ForMember(address => address.Street, opt => opt.MapFrom(dto => dto.Street))
                .ForMember(address => address.HouseNumber, opt => opt.MapFrom(dto => dto.HouseNumber))
                .ForMember(address => address.PostalCode, opt => opt.MapFrom(dto => dto.PostalCode))
                .ForMember(address => address.Note, opt => opt.MapFrom(dto => dto.Note));
        }
    }

}
