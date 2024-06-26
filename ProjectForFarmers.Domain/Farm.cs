﻿using FarmersMarketplace.Domain.Accounts;
using FarmersMarketplace.Domain.Feedbacks;
using FarmersMarketplace.Domain.Payment;

namespace FarmersMarketplace.Domain
{
    public class Farm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string? AdditionalPhone { get; set; }
        public string? FirstSocialPageUrl { get; set; }
        public string? SecondSocialPageUrl { get; set; }
        public List<string> ImagesNames { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual Guid OwnerId { get; set; }
        public Farmer Owner { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public Guid? ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual List<MonthStatistic> Statistics {  get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<FarmLog> Logs { get; set; }
        public List<Guid>? Categories { get; set; }
        public List<Guid>? Subcategories { get; set; }
        public List<ReceivingMethod>? ReceivingMethods { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }
        public Guid? PaymentDataId { get; set; }
        public ProducerPaymentData? PaymentData { get; set; }
        public float Rating { get; set; }
        public Guid FeedbacksId { get; set; }
        public virtual ProducerFeedbackCollection? Feedbacks { get; set; }
    }
}
