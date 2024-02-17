using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.DataTransferObjects.Product
{
    public class ExportProductsDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ProductFilter? Filter { get; set; }
    }

}
