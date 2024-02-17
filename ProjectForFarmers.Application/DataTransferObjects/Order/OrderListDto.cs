using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.DataTransferObjects.Order
{
    public class OrderListDto
    {
        public List<Guid> OrderIds { get; set; }
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
    }
}