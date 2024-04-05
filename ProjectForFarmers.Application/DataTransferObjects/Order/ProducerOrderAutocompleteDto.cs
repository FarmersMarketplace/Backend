using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class ProducerOrderAutocompleteDto
    {
        public Producer Producer { get; set; }
        public Guid ProducerId { get; set; }
        public string Query { get; set; }
        public int Count { get; set; }
    }

}
