using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.ViewModels.Account
{
    public class CustomerOrderDetailsVm
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone {  get; set; }
        public string AdditionalPhone { get; set; }
        public CustomerAddressVm? Address { get; set; }
        public CustomerPaymentDataVm PaymentData { get; set; }
    }

}
