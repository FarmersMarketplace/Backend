using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public decimal PaymentTotal { get; set; }
        public PaymentType PaymentType { get; set; }

    }

}
