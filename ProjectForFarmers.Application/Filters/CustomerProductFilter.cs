using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.Filters
{
    public class CustomerProductFilter
    {
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
        public uint? MaxCount { get; set; }
        public uint? MinCount { get; set; }
        public DateTime? MaxCreationDate { get; set; }
        public DateTime? MinCreationDate { get; set; }
        public HashSet<ReceivingMethod>? ReceivingMethods { get; set; }
        public bool? OnlyOnlinePayment { get; set; }
        public Producer? Producer { get; set; }
        public HashSet<Guid>? Farms { get; set; }
        public HashSet<Guid>? Sellers { get; set; }
        public string? Region { get; set; }
        public HashSet<Guid>? Subcategories { get; set; }
    }

}
