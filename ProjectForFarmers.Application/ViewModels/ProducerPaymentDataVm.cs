using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels
{
    public class ProducerPaymentDataVm
    {
        public string CardNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankUSREOU { get; set; }
        public string BIC { get; set; }
        public string CardExpirationYear { get; set; }
        public string CardExpirationMonth { get; set; }
        public string CVV { get; set; }
    }

}
