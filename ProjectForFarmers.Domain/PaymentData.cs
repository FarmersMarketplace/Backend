using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class PaymentData
    {
        public Guid Id { get; set; }
        public string CardNumber { get; set; }
        public string AccountNumber { get; set; }
        public string BankUSREOU {  get; set; }
        public string BIC {  get; set; }
        public string HolderFullName { get; set; }
    }

}
