using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Farm
{
    public class UpdateFarmPaymentDataDto
    {
        public Guid FarmId { get; set; }
        public ProducerPaymentDataDto PaymentData { get; set; }
        public bool HasOnlinePayment { get; set; }
    }

}
