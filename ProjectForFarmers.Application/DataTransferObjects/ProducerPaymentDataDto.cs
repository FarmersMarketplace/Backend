using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Application.DataTransferObjects
{
    public class ProducerPaymentDataDto
    {
        public bool HasOnlinePayment { get; set; }
        public string CardNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankUSREOU { get; set; }
        public string BIC { get; set; }
        public string CardExpirationYear { get; set; }
        public string CardExpirationMonth { get; set; }
        public MainPaymentData? MainPaymentData { get; set; }

        public ProducerPaymentDataDto()
        {
            MainPaymentData = MainPaymentData ?? Domain.Payment.MainPaymentData.Card;
        }
    }

}
