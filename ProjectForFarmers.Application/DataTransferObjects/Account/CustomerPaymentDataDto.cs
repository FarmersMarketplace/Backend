namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class CustomerPaymentDataDto
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationYear { get; set; }
        public string CardExpirationMonth { get; set; }
    }
}