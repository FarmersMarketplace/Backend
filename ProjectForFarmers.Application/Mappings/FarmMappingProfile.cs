using AutoMapper;
using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Application.ViewModels;
using FarmersMarketplace.Application.ViewModels.Category;
using FarmersMarketplace.Application.ViewModels.Farm;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using System.Net;
using Address = FarmersMarketplace.Domain.Address;
using DayOfWeek = FarmersMarketplace.Domain.DayOfWeek;

namespace FarmersMarketplace.Application.Mappings
{
    public class FarmMappingProfile : Profile
    {
        public FarmMappingProfile()
        {
            MapCreateFarmDtoToFarm();
            MapFarmToFarmLookupVm();
            MapFarmToFarmVm();
        }

        private void MapFarmToFarmVm()
        {
            CreateMap<Farm, FarmVm>()
                .ForMember(vm => vm.Id, opt => opt.MapFrom(farm => farm.Id))
                .ForMember(vm => vm.Name, opt => opt.MapFrom(farm => farm.Name))
                .ForMember(vm => vm.Description, opt => opt.MapFrom(farm => farm.Description))
                .ForMember(vm => vm.ContactEmail, opt => opt.MapFrom(farm => farm.ContactEmail))
                .ForMember(vm => vm.OwnerName, opt => opt.MapFrom(farm => farm.Owner.Name + " " + farm.Owner.Surname))
                .ForMember(vm => vm.Address, opt => opt.MapFrom(farm => farm.Address))
                .ForMember(vm => vm.Schedule, opt => opt.MapFrom(farm => farm.Schedule))
                .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(farm => farm.PaymentData))
                .ForMember(vm => vm.ReceivingMethods, opt => opt.MapFrom(farm => farm.ReceivingMethods))
                .ForMember(vm => vm.HasDelivery, opt => opt.MapFrom(farm => farm.PaymentTypes != null && farm.PaymentTypes.Contains(PaymentType.Online)))
                .ForMember(vm => vm.FirstSocialPageUrl, opt => opt.MapFrom(farm => farm.FirstSocialPageUrl))
                .ForMember(vm => vm.SecondSocialPageUrl, opt => opt.MapFrom(farm => farm.SecondSocialPageUrl))
                .ForMember(vm => vm.ImagesNames, opt => opt.MapFrom(farm => farm.ImagesNames))
                .ForMember(vm => vm.Categories, opt => opt.MapFrom(farm => new List<CategoryLookupVm>()))
                .ForMember(vm => vm.Subcategories, opt => opt.MapFrom(farm => new List<SubcategoryVm>()))
                .ForMember(vm => vm.Logs, opt => opt.MapFrom(farm => new List<FarmLogVm>()));
        }

        private void MapCreateFarmDtoToFarm()
        {
            CreateMap<CreateFarmDto, Farm>()
                .ForMember(farm => farm.Id, opt => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(farm => farm.Name, opt => opt.MapFrom(dto => dto.Name))
                .ForMember(farm => farm.Description, opt => opt.MapFrom(dto => dto.Description))
                .ForMember(farm => farm.ContactEmail, opt => opt.MapFrom(dto => string.Empty))
                .ForMember(farm => farm.ContactPhone, opt => opt.MapFrom(dto => dto.ContactPhone))
                .ForMember(farm => farm.CreationDate, opt => opt.MapFrom(dto => DateTime.UtcNow))
                .ForMember(farm => farm.OwnerId, opt => opt.MapFrom(dto => dto.OwnerId))
                .ForMember(farm => farm.FirstSocialPageUrl, opt => opt.MapFrom(dto => dto.FirstSocialPageUrl))
                .ForMember(farm => farm.SecondSocialPageUrl, opt => opt.MapFrom(dto => dto.SecondSocialPageUrl))
                .ForMember(farm => farm.Address, opt => opt.MapFrom(dto => dto.Address))
                .ForMember(farm => farm.ImagesNames, opt => opt.MapFrom(dto => new List<string>()))
                .ForMember(farm => farm.Logs, opt => opt.MapFrom(dto => new List<FarmLog>()))
                .ForMember(farm => farm.Categories, opt => opt.MapFrom(dto => new List<Guid>()))
                .ForMember(farm => farm.Subcategories, opt => opt.MapFrom(dto => new List<Guid>()))
                .ForMember(farm => farm.Schedule, opt => opt.MapFrom(dto => dto.Schedule))
                .ForMember(farm => farm.ReceivingMethods, opt => opt.MapFrom(dto => new List<ReceivingMethod>()))
                .ForMember(farm => farm.PaymentTypes, opt => opt.MapFrom(dto => new List<PaymentType>() { PaymentType.Cash }));
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
