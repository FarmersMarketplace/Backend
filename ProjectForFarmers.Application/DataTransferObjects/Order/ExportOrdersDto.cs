using ProjectForFarmers.Application.Filters;
using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.DataTransferObjects.Order
{
    public class ExportOrdersDto
    {
        public Guid ProducerId { get; set; }
        public Producer Producer { get; set; }
        public OrderFilter? Filter { get; set; }
    }

}
