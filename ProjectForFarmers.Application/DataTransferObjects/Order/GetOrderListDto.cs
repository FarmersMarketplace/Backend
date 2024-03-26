using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class GetOrderListDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public OrderFilter? Filter { get; set; }
        public DateTime Cursor {  get; set; }
        public int PageSize { get; set; }
        public string? Query { get; set; }
    }

}
