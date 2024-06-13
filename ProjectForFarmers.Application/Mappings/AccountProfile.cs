using AutoMapper;
using FarmersMarketplace.Application.ViewModels.Account;
using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Application.Mappings
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
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
                .ForMember(vm => vm.Schedule, opt => opt.MapFrom(seller => seller.Schedule))
                .ForMember(vm => vm.PaymentData.HasOnlinePayment, opt => opt.MapFrom(seller => seller.PaymentTypes != null
                && seller.PaymentTypes.Contains(PaymentType.Online)));
        }

        private void MapCustomerToCustomerVm()
        {
            CreateMap<Customer, CustomerVm>()
                .ForMember(vm => vm.PaymentData, opt => opt.MapFrom(customer => customer.PaymentData))
                .ForMember(vm => vm.Address, opt => opt.MapFrom(customer => customer.Address));
        }

    }

}
