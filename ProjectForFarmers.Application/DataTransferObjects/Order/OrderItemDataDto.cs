namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class OrderItemDataDto
    {
        public Guid ProductId { get; set; }
        public uint Count { get; set; }
    }

}
