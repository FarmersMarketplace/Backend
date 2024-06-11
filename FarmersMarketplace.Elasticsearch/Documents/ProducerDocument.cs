using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Elasticsearch.Documents
{
    public class ProducerDocument
    {
        public Producer Producer { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Region { get; set; }
        public HashSet<Guid> Subcategories { get; set; }
        public string ImageName { get; set; }
        public uint FeedbacksCount { get; set; }
        public float Rating { get; set; }
    }
}
