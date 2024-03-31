using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Filters
{
    public class ProducerProductFilter
    {
        public List<ProductStatus>? Statuses { get; set; }
        public List<Guid>? Subcategories { get; set; }
        public DateTime? MinCreationDate { get; set; }
        public DateTime? MaxCreationDate { get; set; }
        public List<string>? UnitsOfMeasurement { get; set; }
        public uint? MinRest { get; set; }
        public uint? MaxRest { get; set; }
    }

}