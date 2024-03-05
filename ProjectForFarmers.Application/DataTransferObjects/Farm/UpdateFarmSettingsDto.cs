using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Farm
{
    public class UpdateFarmSettingsDto
    {
        public Guid FarmId { get; set; }
        public PaymentDataDto PaymentData { get; set; }
        public bool HasOnlinePayment { get; set; }
    }

}
