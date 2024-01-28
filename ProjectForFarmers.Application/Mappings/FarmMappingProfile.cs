using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using ProjectForFarmers.Application.DataTransferObjects.Farm;
using ProjectForFarmers.Application.ViewModels.Farm;
using ProjectForFarmers.Domain;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayOfWeek = ProjectForFarmers.Domain.DayOfWeek;

namespace ProjectForFarmers.Application.Mappings
{
    public class FarmMappingProfile : Profile
    {
        public FarmMappingProfile()
        {
            MapAddressDtoToAddress();
            MapDayOfWeekDtoToDayOfWeek();
            MapScheduleDtoToSchedule();
            MapCreateFarmDtoToFarm();
            MapUpdateFarmDtoToFarm();
            MapFarmToFarmLookupVm();
        }

        private void MapAddressDtoToAddress()
        {
            CreateMap<AddressDto, Address>()
                .ForMember(address => address.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(address => address.Region, opt => opt.MapFrom(dto => dto.Region))
                .ForMember(address => address.District, opt => opt.MapFrom(dto => dto.District))
                .ForMember(address => address.Settlement, opt => opt.MapFrom(dto => dto.Settlement))
                .ForMember(address => address.Street, opt => opt.MapFrom(dto => dto.Street))
                .ForMember(address => address.HouseNumber, opt => opt.MapFrom(dto => dto.HouseNumber))
                .ForMember(address => address.PostalCode, opt => opt.MapFrom(dto => dto.PostalCode))
                .ForMember(address => address.Note, opt => opt.MapFrom(dto => dto.Note));
        }

        private void MapDayOfWeekDtoToDayOfWeek()
        {
            CreateMap<DayOfWeekDto, DayOfWeek>()
                .ForMember(dayOfWeek => dayOfWeek.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(dayOfWeek => dayOfWeek.StartHour, opt => opt.MapFrom(dto => dto.StartHour))
                .ForMember(dayOfWeek => dayOfWeek.StartMinute, opt => opt.MapFrom(dto => dto.StartMinute))
                .ForMember(dayOfWeek => dayOfWeek.EndHour, opt => opt.MapFrom(dto => dto.EndHour))
                .ForMember(dayOfWeek => dayOfWeek.EndMinute, opt => opt.MapFrom(dto => dto.EndMinute));
        }

        private void MapScheduleDtoToSchedule()
        {
            CreateMap<ScheduleDto, Schedule>()
                .ForMember(schedule => schedule.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(schedule => schedule.Monday, opt => opt.MapFrom(dto => dto.Monday))
                .ForMember(schedule => schedule.Tuesday, opt => opt.MapFrom(dto => dto.Tuesday))
                .ForMember(schedule => schedule.Wednesday, opt => opt.MapFrom(dto => dto.Wednesday))
                .ForMember(schedule => schedule.Thursday, opt => opt.MapFrom(dto => dto.Thursday))
                .ForMember(schedule => schedule.Friday, opt => opt.MapFrom(dto => dto.Friday))
                .ForMember(schedule => schedule.Saturday, opt => opt.MapFrom(dto => dto.Saturday))
                .ForMember(schedule => schedule.Sunday, opt => opt.MapFrom(dto => dto.Sunday));
        }

        private void MapCreateFarmDtoToFarm()
        {
            CreateMap<CreateFarmDto, Farm>()
                .ForMember(farm => farm.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(farm => farm.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(farm => farm.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(farm => farm.ContactEmail, opt => opt.MapFrom(dto => dto.ContactEmail))
                .ForMember(farm => farm.ContactPhone, opt => opt.MapFrom(dto => dto.ContactPhone))
                .ForMember(farm => farm.CreationDate, opt => opt.MapFrom(dto => DateTime.UtcNow))
                .ForMember(farm => farm.OwnerId, opt => opt.MapFrom(dto => dto.OwnerId))
                .ForMember(farm => farm.WebsiteUrl, opt => opt.MapFrom(dto => dto.WebsiteUrl))
                .ForMember(farm => farm.Address, opt => opt.MapFrom(dto => dto.Address))
                .ForMember(farm => farm.Schedule, opt => opt.MapFrom(dto => dto.Schedule));
        }

        private void MapUpdateFarmDtoToFarm()
        {
            CreateMap<UpdateFarmDto, Farm>()
                .ForMember(farm => farm.Id, opt => opt.MapFrom(dto => dto.FarmId))
                .ForMember(farm => farm.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(farm => farm.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(farm => farm.ContactEmail, opt => opt.MapFrom(dto => dto.ContactEmail))
                .ForMember(farm => farm.ContactPhone, opt => opt.MapFrom(dto => dto.ContactPhone))
                .ForMember(farm => farm.WebsiteUrl, opt => opt.MapFrom(dto => dto.WebsiteUrl))
                .ForMember(farm => farm.Address, opt => opt.MapFrom(dto => dto.Address))
                .ForMember(farm => farm.Schedule, opt => opt.MapFrom(dto => dto.Schedule));
        }

        private void MapFarmToFarmLookupVm()
        {
            CreateMap<Farm, FarmLookupVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(farm => farm.Id))
                .ForMember(farm => farm.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(farm => farm.AvatarName, opt => opt.MapFrom(dto => 
                (dto.ImagesNames == null || dto.ImagesNames.Count <= 0) ? "" : dto.ImagesNames[0]));
        }
    }

}
