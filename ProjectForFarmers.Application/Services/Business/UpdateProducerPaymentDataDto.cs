using FarmersMarketplace.Application.DataTransferObjects;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Services.Business
{
    public class UpdateProducerPaymentDataDto
    {
        public Producer Producer { get; set; }
        public bool HasOnlinePayment { get; set; }
        public ProducerPaymentDataDto PaymentDataDto { get; set; }
    }
}