namespace FarmersMarketplace.Domain.Orders
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        public uint Count { get; set; }
        public decimal PricePerOne { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
