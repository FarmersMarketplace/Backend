namespace FarmersMarketplace.Application.Filters
{
    public class ProducerProductFilter
    {
        public List<Guid>? CategoryIds { get; set; }
        public List<Guid>? SubcategoryIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string>? UnitsOfMeasurement { get; set; }
        public uint? MinRest { get; set; }
        public uint? MaxRest { get; set; }
    }

}