namespace FarmersMarketplace.Application.ViewModels.Order
{
    public class OrderItemVm
    {
        public Guid Id { get; set; }
        public string? ImageName { get; set; }
        public string Name { get; set; }
        public string ArticleNumber { get; set; }
        public uint Count { get; set; }
        public decimal PricePerOne { get; set; }
        public decimal TotalPrice { get; set; }
        public string UnitOfMeasurement { get; set; }
    }

}
