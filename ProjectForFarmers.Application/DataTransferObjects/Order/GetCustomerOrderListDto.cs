namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class GetCustomerOrderListDto
    {
        public Guid CustomerId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Query { get; set; }
    }
}
