using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.ViewModels.Producers
{
    public class ProducerLookupVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Producer Producer { get; set; }
        public string ImageName { get; set; }
        public uint FeedbacksCount { get; set; }
        public float Rating { get; set; }
    }
}
