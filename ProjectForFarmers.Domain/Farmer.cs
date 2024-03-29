﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class Farmer : Account
    {
        public string? AvatarName { get; set; }
        public Guid? PaymentDataId { get; set; }
        public ProducerPaymentData? PaymentData { get; set; }
        public Guid? AddressId { get; set; }
        public Address? Address { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }
    }

}
