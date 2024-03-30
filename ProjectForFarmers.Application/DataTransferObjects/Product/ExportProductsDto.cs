using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Product
{
    public class ExportProductsDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ProducerProductFilter? Filter { get; set; }
    }

}
