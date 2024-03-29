using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Domain.Account
{
    public class Seller : Account
    {
        public List<string> ImagesNames { get; set; }
        public Guid? PaymentDataId { get; set; }
        public ProducerPaymentData PaymentData { get; set; }
        public Guid? AddressId { get; set; }
        public Address Address { get; set; }
        public Guid? ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
        public string FirstSocialPageUrl { get; set; }
        public string SecondSocialPageUrl { get; set; }
        public List<ReceivingMethod> ReceivingMethods { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<Guid> Categories { get; set; }
        public List<Guid> Subcategories { get; set; }
        public virtual List<MonthStatistic> Statistics { get; set; }
        public virtual List<Product> Products { get; set; }
    }

}
