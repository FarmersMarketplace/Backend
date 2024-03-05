namespace FarmersMarketplace.Domain
{
    public class Farm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string? FirstSocialPageUrl { get; set; }
        public string? SecondSocialPageUrl { get; set; }
        public List<string> ImagesNames { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual Guid OwnerId { get; set; }
        public Account Owner { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public Guid ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual List<MonthStatistic> Statistics {  get; set; }
        public virtual List<FarmLog> Logs { get; set; }
        public List<Guid>? Categories { get; set; }
        public List<Guid>? Subcategories { get; set; }
        public List<ReceivingMethod>? ReceivingMethods { get; set; }
        public List<PaymentType>? PaymentTypes { get; set; }
        public Guid? PaymentDataId { get; set; }
        public PaymentData? PaymentData { get; set; }
    }
}
