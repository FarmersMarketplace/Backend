using FarmersMarketplace.Application.DataTransferObjects.Account;
using FarmersMarketplace.Domain;
using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ReceivingMethod ReceivingMethod { get; set; }
        public CustomerAddressDto? DeliveryPoint { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAdditionalPhone { get; set; }
        public PaymentType PaymentType { get; set; }
        public CustomerPaymentDataDto? PaymentData { get; set; }
        public List<Guid> OrderItemsIds { get; set; }
        public DateTime ReceiveDate { get; set; }
    }
}
