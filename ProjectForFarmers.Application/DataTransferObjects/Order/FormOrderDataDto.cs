using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class FormOrderDataDto
    {
        public Guid Id { get; set; }
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }

        public List<OrderItemDataDto> Items { get; set; }
    }

}
