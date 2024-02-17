using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.DataTransferObjects.Order
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
