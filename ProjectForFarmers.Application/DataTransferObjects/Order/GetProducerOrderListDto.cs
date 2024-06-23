using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class GetProducerOrderListDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ProducerOrderFilter? Filter { get; set; }
        public int Page {  get; set; }
        public int PageSize { get; set; }
        public string? Query { get; set; }
    }

}
