namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class FarmerPaymentDataDto : ProducerPaymentDataDto
    {
        public bool HasOnlinePayment { get; set; }
    }
}