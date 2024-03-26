using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Dashboard
{
    public class GetCustomerDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
    }

}
