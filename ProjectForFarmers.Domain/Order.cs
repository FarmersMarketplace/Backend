namespace ProjectForFarmers.Domain
{
    public class Order
    {
        public Guid Id { get; set; }
        public uint Number { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public decimal TotalPayment { get; set; }
        public PaymentType PaymentType { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public ReceivingType ReceivingType { get; set; }
        public Guid SupplyPointId { get; set; }
        public Guid? DeliveryPointId { get; set; }
        public Address SupplyPoint { get; set; }
        public Address? DeliveryPoint { get; set; }
        public Producer Producer { get; set; }
        public OrderStatus Status { get; set; }
        public Guid ProducerId { get; set; }
        public Guid CustomerId { get; set; }
        public virtual Account Customer { get; set; }
        public List<OrderItem> Items { get; set; }
    }

}
