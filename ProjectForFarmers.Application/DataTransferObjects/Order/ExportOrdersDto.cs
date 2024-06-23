using FarmersMarketplace.Application.Filters;
using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Order
{
    public class ExportOrdersDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public ProducerOrderFilter? Filter { get; set; }
    }

}
