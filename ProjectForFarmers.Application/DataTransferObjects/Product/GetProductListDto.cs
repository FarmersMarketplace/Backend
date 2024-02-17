using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.DataTransferObjects.Product
{
    public class GetProductListDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; } 
        public ProductFilter? Filter { get; set; }
        public DateTime Cursor { get; set; }
        public int PageSize { get; set; }
        public string? Query { get; set; }
    }

}
