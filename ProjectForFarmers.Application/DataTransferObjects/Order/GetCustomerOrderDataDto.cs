using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class GetCustomerOrderDataDto
    {
        public Guid Id { get; set; }
        public List<OrderItemDataDto> Items { get; set; }
    }

}
