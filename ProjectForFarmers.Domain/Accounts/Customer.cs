using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Domain.Account
{
    public class Customer : Account
    {
        public string AvatarName { get; set; }
        public Guid? PaymentDataId { get; set; }
        public CustomerPaymentData PaymentData { get; set; }
        public Guid? AddressId { get; set; }
        public CustomerAddress Address { get; set; }
        public List<Guid> FavouriteProducts { get; set; }
    }

}
