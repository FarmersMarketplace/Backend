using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Application.ViewModels.Farm;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Payment;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Application.Mappings
{
    public class DataProfile : Profile
    {
        public DataProfile()
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
            CreateMap<CustomerPaymentDataDto, CustomerPaymentData>()
                .ForMember(data => data.Id, opt => opt.MapFrom(dto => Guid.NewGuid()));
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
                .ForMember(data => data.Id, opt => opt.MapFrom(dto => Guid.NewGuid()));
        }
        private void MapDayOfWeekDtoToDayOfWeek()
        {
            CreateMap<DayOfWeekDto, DayOfWeek>()
                .ForMember(dayOfWeek => dayOfWeek.Id, opt => opt.MapFrom(dto => Guid.NewGuid()));
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
            CreateMap<ProducerPaymentData, ProducerPaymentDataVm>();
        }

        private void MapAddressToAddressVm()
        {
            CreateMap<Address, AddressVm>();
        }

        private void MapDayOfWeekToDayOfWeekVm()
        {
            CreateMap<DayOfWeek, DayOfWeekVm>();
        }

        private void MapAddressDtoToAddress()
        {
            CreateMap<AddressDto, Address>()
                .ForMember(address => address.Id, opt => opt.MapFrom(dto => Guid.NewGuid()));
        }
    }

}
