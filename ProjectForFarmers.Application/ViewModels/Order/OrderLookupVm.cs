using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Order
{
    public class OrderLookupVm
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public decimal TotalPayment { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderStatus Status { get; set; }
    }

}
