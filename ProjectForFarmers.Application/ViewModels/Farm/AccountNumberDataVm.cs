using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Farm
{
    public class AccountNumberDataVm
    {
        public bool HasOnlinePayment { get; set; }
        public string AccountNumber { get; set; }
        public string BankUSREOU { get; set; }
        public string BIC { get; set; }
    }

}
