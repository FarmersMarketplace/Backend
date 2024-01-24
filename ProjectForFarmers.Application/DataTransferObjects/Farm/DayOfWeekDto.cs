using AutoMapper;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayOfWeek = ProjectForFarmers.Domain.DayOfWeek;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class DayOfWeekDto : IMapWith<DayOfWeek>
    {
        public string StartHour { get; set; }
        public string StartMinute { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DayOfWeekDto, DayOfWeek>()
                .ForMember(dayOfWeek => dayOfWeek.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(dayOfWeek => dayOfWeek.StartHour, opt => opt.MapFrom(dto => dto.StartHour))
                .ForMember(dayOfWeek => dayOfWeek.StartMinute, opt => opt.MapFrom(dto => dto.StartMinute))
                .ForMember(dayOfWeek => dayOfWeek.EndHour, opt => opt.MapFrom(dto => dto.EndHour))
                .ForMember(address => address.EndMinute, opt => opt.MapFrom(dto => dto.EndMinute));
        }
    }

}
