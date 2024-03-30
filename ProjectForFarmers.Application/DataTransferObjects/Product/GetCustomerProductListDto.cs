using FarmersMarketplace.Application.Filters;

namespace FarmersMarketplace.Application.DataTransferObjects.Product
{
    public class GetCustomerProductListDto
    {
        public string? Query { get; set; }
        public CustomerProductFilter? Filter { get; set; }
        public int Page {  get; set; }
        public int CountPerPage { get; set; }
    }

}
