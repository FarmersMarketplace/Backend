using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class CustomerPaymentData
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationYear { get; set; }
        public string CardExpirationMonth { get; set; }
        public string CVV { get; set; }
    }

}
