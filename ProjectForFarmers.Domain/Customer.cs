using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class Customer : Account
    {
        public string? AvatarName { get; set; }
        public Guid? PaymentDataId { get; set; }
        public CustomerPaymentData? PaymentData { get; set; }
        public Guid? AddressId { get; set; }
        public CustomerAddress? Address { get; set; }
    }

}
