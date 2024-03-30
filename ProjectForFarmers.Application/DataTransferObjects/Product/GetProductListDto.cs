using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Product
{
    public class GetProductListDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; } 
        public ProducerProductFilter? Filter { get; set; }
        public DateTime Cursor { get; set; }
        public int PageSize { get; set; }
        public string? Query { get; set; }
    }

}
