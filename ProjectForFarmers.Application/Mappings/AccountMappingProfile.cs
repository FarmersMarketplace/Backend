using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Application.ViewModels.Subcategory;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Mappings
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            MapCustomerToCustomerVm();
            MapSellerToSellerVm();
            MapFarmerToFarmerVm();
        }

        private void MapFarmerToFarmerVm()
        {
            CreateMap<Farmer, FarmerVm>()
             .ForMember(vm => vm.Name, opt => opt.MapFrom(farmer => farmer.Name))
             .ForMember(vm => vm.Surname, opt => opt.MapFrom(farmer => farmer.Surname))
             .ForMember(vm => vm.Patronymic, opt => opt.MapFrom(farmer => farmer.Patronymic))
             .ForMember(vm => vm.Email, opt => opt.MapFrom(farmer => farmer.Email))
             .ForMember(vm => vm.Phone, opt => opt.MapFrom(farmer => farmer.Phone))
             .ForMember(vm => vm.Gender, opt => opt.MapFrom(farmer => farmer.Gender))
             .ForMember(vm => vm.DateOfBirth, opt => opt.MapFrom(farmer => farmer.DateOfBirth))
             .ForMember(vm => vm.AvatarName, opt => opt.MapFrom(farmer => farmer.AvatarName))
             .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(farmer => farmer.PaymentData))
             .ForMember(vm => vm.Address, opt => opt.MapFrom(farmer => farmer.Address));
        }

        private void MapSellerToSellerVm()
        {
            CreateMap<Seller, SellerVm>()
                .ForMember(vm => vm.Name, opt => opt.MapFrom(seller => seller.Name))
                .ForMember(vm => vm.Surname, opt => opt.MapFrom(seller => seller.Surname))
                .ForMember(vm => vm.Patronymic, opt => opt.MapFrom(seller => seller.Patronymic))
                .ForMember(vm => vm.Email, opt => opt.MapFrom(seller => seller.Email))
                .ForMember(vm => vm.Phone, opt => opt.MapFrom(seller => seller.Phone))
                .ForMember(vm => vm.Password, opt => opt.MapFrom(seller => seller.Password))
                .ForMember(vm => vm.Gender, opt => opt.MapFrom(seller => seller.Gender))
                .ForMember(vm => vm.DateOfBirth, opt => opt.MapFrom(seller => seller.DateOfBirth))
                .ForMember(vm => vm.ImagesNames, opt => opt.MapFrom(seller => seller.ImagesNames))
                .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(seller => seller.PaymentData))
                .ForMember(vm => vm.Address, opt => opt.MapFrom(seller => seller.Address))
                .ForMember(vm => vm.Schedule, opt => opt.MapFrom(seller => seller.Schedule))
                .ForMember(vm => vm.FirstSocialPageUrl, opt => opt.MapFrom(seller => seller.FirstSocialPageUrl))
                .ForMember(vm => vm.SecondSocialPageUrl, opt => opt.MapFrom(seller => seller.SecondSocialPageUrl))
                .ForMember(vm => vm.ReceivingMethods, opt => opt.MapFrom(seller => seller.ReceivingMethods))
                .ForMember(vm => vm.PaymentTypes, opt => opt.MapFrom(seller => seller.PaymentTypes));
        }

        private void MapCustomerToCustomerVm()
        {
            CreateMap<Customer, CustomerVm>()
                .ForMember(vm => vm.Name, opt => opt.MapFrom(customer => customer.Name))
                .ForMember(vm => vm.Surname, opt => opt.MapFrom(customer => customer.Surname))
                .ForMember(vm => vm.Patronymic, opt => opt.MapFrom(customer => customer.Patronymic))
                .ForMember(vm => vm.Email, opt => opt.MapFrom(customer => customer.Email))
                .ForMember(vm => vm.Phone, opt => opt.MapFrom(customer => customer.Phone))
                .ForMember(vm => vm.Gender, opt => opt.MapFrom(customer => customer.Gender))
                .ForMember(vm => vm.DateOfBirth, opt => opt.MapFrom(customer => customer.DateOfBirth))
                .ForMember(vm => vm.AvatarName, opt => opt.MapFrom(customer => customer.AvatarName))
                .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(customer => customer.PaymentData))
                .ForMember(vm => vm.Address, opt => opt.MapFrom(customer => customer.Address));
        }

    }

}
