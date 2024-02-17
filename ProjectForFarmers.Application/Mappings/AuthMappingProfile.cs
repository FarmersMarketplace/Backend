using AutoMapper;
using ProjectForFarmers.Application.DataTransferObjects.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Mappings
{
    public class AuthMappingProfile : Profile
    {
        public AuthMappingProfile()
        {
            MapRegisterDtoToAccount();
        }

        private void MapRegisterDtoToAccount()
        {
            CreateMap<RegisterDto, Domain.Account>()
                .ForMember(account => account.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(account => account.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(account => account.Surname, opt => opt.MapFrom(dto => dto.Surname))
                .ForMember(account => account.Email, opt => opt.MapFrom(dto => dto.Email))
                .ForMember(account => account.Password, opt => opt.MapFrom(dto => dto.Password))
                .ForMember(account => account.Role, opt => opt.MapFrom(dto => dto.Role));
        }
    }

}
