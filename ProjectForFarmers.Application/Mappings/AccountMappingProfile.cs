using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Domain.Accounts;

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
             .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(farmer => farmer.PaymentData))
             .ForMember(vm => vm.Address, opt => opt.MapFrom(farmer => farmer.Address));
        }

        private void MapSellerToSellerVm()
        {
            CreateMap<Seller, SellerVm>()
                .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(seller => seller.PaymentData))
                .ForMember(vm => vm.Address, opt => opt.MapFrom(seller => seller.Address))
                .ForMember(vm => vm.Schedule, opt => opt.MapFrom(seller => seller.Schedule));
        }

        private void MapCustomerToCustomerVm()
        {
            CreateMap<Customer, CustomerVm>()
                .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(customer => customer.PaymentData))
                .ForMember(vm => vm.Address, opt => opt.MapFrom(customer => customer.Address));
        }

    }

}
