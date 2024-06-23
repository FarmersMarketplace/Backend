namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class CustomerOrderAutocompleteDto
    {
        public Guid CustomerId { get; set; }
        public string Query { get; set; }
        public int Count { get; set; }
    }
}
