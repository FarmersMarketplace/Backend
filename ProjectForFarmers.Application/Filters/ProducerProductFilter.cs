using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Filters
{
    public class ProducerProductFilter
    {
        public HashSet<ProductStatus>? Statuses { get; set; }
        public HashSet<Guid>? Subcategories { get; set; }
        public DateTime? MinCreationDate { get; set; }
        public DateTime? MaxCreationDate { get; set; }
        public HashSet<string>? UnitsOfMeasurement { get; set; }
        public uint? MinRest { get; set; }
        public uint? MaxRest { get; set; }
        public bool? HasOnlinePayment { get; set; }
    }

}