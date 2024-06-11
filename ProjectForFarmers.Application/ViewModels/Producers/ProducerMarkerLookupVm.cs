using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.ViewModels.Producers
{
    public class ProducerMarkerLookupVm
    {
        public Guid Id { get; set; }
        public Producer Producer { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
