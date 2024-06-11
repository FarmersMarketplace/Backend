using FarmersMarketplace.Application.Filters;

namespace FarmersMarketplace.Application.DataTransferObjects.Producers
{
    public class GetProducerMarkersDto
    {
        public double MaxLatitude { get; set; }
        public double MaxLongitude { get; set; }
        public double MinLatitude { get; set; }
        public double MinLongitude { get; set; }
        public ProducerFilter? Filter { get; set; }
    }
}
