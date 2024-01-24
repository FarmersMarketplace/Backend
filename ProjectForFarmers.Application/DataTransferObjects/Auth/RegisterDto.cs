using AutoMapper;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.DataTransferObjects.Auth
{
    public class RegisterDto : IMapWith<Domain.Account>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword{ get; set; }
        public Role Role { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterDto, Domain.Account>()
                .ForMember(account => account.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(account => account.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(account => account.Surname, opt => opt.MapFrom(dto => dto.Surname))
                .ForMember(account => account.Email, opt => opt.MapFrom(dto => dto.Email))
                .ForMember(account => account.Password, opt => opt.MapFrom(dto => dto.Password))
                .ForMember(account => account.Role, opt => opt.MapFrom(dto => dto.Role));
        }
    }
}
