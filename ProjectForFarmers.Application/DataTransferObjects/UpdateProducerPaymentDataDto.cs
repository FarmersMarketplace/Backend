using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects
{
    public class UpdateProducerPaymentDataDto
    {
        public Guid ProducerId { get; set; }
        public ProducerPaymentDataDto PaymentData { get; set; }
        public bool HasOnlinePayment { get; set; }
    }

}
