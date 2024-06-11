using FarmersMarketplace.Application.DataTransferObjects.Producers;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Farm
{
    public class FarmPaymentDataDto : ProducerPaymentDataDto
    {
        public Guid FarmId { get; set; }
    }

}
