using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Filters
{
    public class ProducerFilter
    {
        public Producer? Producer { get; set; }
        public HashSet<Guid>? Farms { get; set; }
        public HashSet<Guid>? Sellers { get; set; }
        public string? Region { get; set; }
        public HashSet<Guid>? Subcategories { get; set; }
    }
}
