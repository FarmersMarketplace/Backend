using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public uint Count { get; set; }
    }

}
