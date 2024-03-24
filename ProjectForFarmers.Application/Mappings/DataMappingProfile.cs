﻿using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Application.ViewModels.Farm;
using FarmersMarketplace.Domain;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Application.Mappings
{
    public class DataMappingProfile : Profile
    {
        public DataMappingProfile()
        {
            MapProducerPaymentDataDtoToProducerPaymentData();
            MapDayOfWeekDtoToDayOfWeek();
            MapScheduleDtoToSchedule();
            MapScheduleToScheduleVm();
            MapProducerPaymentDataToProducerPaymentDataVm();
            MapAddressToAddressVm();
            MapDayOfWeekToDayOfWeekVm();
            MapAddressDtoToAddress();
            MapCustomerPaymentDataToCustomerPaymentDataVm();
            MapCustomerPaymentDataDtoToCustomerPaymentData();
            MapCustomerAddressToCustomerAddressVm();
        }

        private void MapCustomerPaymentDataDtoToCustomerPaymentData()
        {
            CreateMap<CustomerPaymentDataDto, CustomerPaymentData>();
        }

        private void MapCustomerPaymentDataToCustomerPaymentDataVm()
        {
            CreateMap<CustomerPaymentData, CustomerPaymentDataVm>();
        }

        private void MapCustomerAddressToCustomerAddressVm()
        {
            CreateMap<CustomerAddress, CustomerAddressVm>();
        }
        private void MapProducerPaymentDataDtoToProducerPaymentData()
        {
            CreateMap<ProducerPaymentDataDto, ProducerPaymentData>()
                .ForMember(data => data.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(data => data.CardNumber, opt => opt.MapFrom(dto => dto.CardNumber))
                .ForMember(data => data.AccountNumber, opt => opt.MapFrom(dto => dto.AccountNumber))
                .ForMember(data => data.BankUSREOU, opt => opt.MapFrom(dto => dto.BankUSREOU))
                .ForMember(data => data.BIC, opt => opt.MapFrom(dto => dto.BIC))
                .ForMember(data => data.HolderFullName, opt => opt.MapFrom(dto => dto.HolderFullName));
        }
        private void MapDayOfWeekDtoToDayOfWeek()
        {
            CreateMap<DayOfWeekDto, DayOfWeek>()
                .ForMember(dayOfWeek => dayOfWeek.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(dayOfWeek => dayOfWeek.IsOpened, opt => opt.MapFrom(dto => dto.IsOpened))
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

        private void MapScheduleToScheduleVm()
        {
            CreateMap<Schedule, ScheduleVm>()
                .ForMember(vm => vm.Monday, opt => opt.MapFrom(schedule => schedule.Monday))
                .ForMember(vm => vm.Tuesday, opt => opt.MapFrom(schedule => schedule.Tuesday))
                .ForMember(vm => vm.Wednesday, opt => opt.MapFrom(schedule => schedule.Wednesday))
                .ForMember(vm => vm.Thursday, opt => opt.MapFrom(schedule => schedule.Thursday))
                .ForMember(vm => vm.Friday, opt => opt.MapFrom(schedule => schedule.Friday))
                .ForMember(vm => vm.Saturday, opt => opt.MapFrom(schedule => schedule.Saturday))
                .ForMember(vm => vm.Sunday, opt => opt.MapFrom(schedule => schedule.Sunday));
        }

        private void MapProducerPaymentDataToProducerPaymentDataVm()
        {
            CreateMap<ProducerPaymentData, ProducerPaymentDataVm>()
               .ForMember(vm => vm.CardNumber, opt => opt.MapFrom(data => data.CardNumber))
               .ForMember(vm => vm.AccountNumber, opt => opt.MapFrom(data => data.AccountNumber))
               .ForMember(vm => vm.BankUSREOU, opt => opt.MapFrom(data => data.BankUSREOU))
               .ForMember(vm => vm.BIC, opt => opt.MapFrom(data => data.BIC))
               .ForMember(vm => vm.HolderFullName, opt => opt.MapFrom(data => data.HolderFullName));
        }

        private void MapAddressToAddressVm()
        {
            CreateMap<Address, AddressVm>()
                .ForMember(vm => vm.Region, opt => opt.MapFrom(address => address.Region))
                .ForMember(vm => vm.District, opt => opt.MapFrom(address => address.District))
                .ForMember(vm => vm.Settlement, opt => opt.MapFrom(address => address.Settlement))
                .ForMember(vm => vm.Street, opt => opt.MapFrom(address => address.Street))
                .ForMember(vm => vm.HouseNumber, opt => opt.MapFrom(address => address.HouseNumber))
                .ForMember(vm => vm.PostalCode, opt => opt.MapFrom(address => address.PostalCode))
                .ForMember(vm => vm.Note, opt => opt.MapFrom(address => address.Note))
                .ForMember(vm => vm.Longitude, opt => opt.MapFrom(address => address.Longitude))
                .ForMember(vm => vm.Latitude, opt => opt.MapFrom(address => address.Latitude));
        }

        private void MapDayOfWeekToDayOfWeekVm()
        {
            CreateMap<DayOfWeek, DayOfWeekVm>()
                .ForMember(vm => vm.IsOpened, opt => opt.MapFrom(dayOfWeek => dayOfWeek.IsOpened))
                .ForMember(vm => vm.StartHour, opt => opt.MapFrom(dayOfWeek => dayOfWeek.StartHour))
                .ForMember(vm => vm.StartMinute, opt => opt.MapFrom(dayOfWeek => dayOfWeek.StartMinute))
                .ForMember(vm => vm.EndHour, opt => opt.MapFrom(dayOfWeek => dayOfWeek.EndHour))
                .ForMember(vm => vm.EndMinute, opt => opt.MapFrom(dayOfWeek => dayOfWeek.EndMinute));
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
    }

}