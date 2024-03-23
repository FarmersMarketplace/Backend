namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class FarmerPaymentDataDto
    {
        public bool HasOnlinePayment { get; set; }
        public ProducerPaymentDataDto PaymentData { get; set; }
    }
}