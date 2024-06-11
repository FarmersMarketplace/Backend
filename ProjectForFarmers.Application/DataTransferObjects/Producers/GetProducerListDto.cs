using FarmersMarketplace.Application.Filters;

namespace FarmersMarketplace.Application.DataTransferObjects.Producers
{
    public class GetProducerListDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Query { get; set; }
        public ProducerFilter? Filter { get; set; }
    }
}
