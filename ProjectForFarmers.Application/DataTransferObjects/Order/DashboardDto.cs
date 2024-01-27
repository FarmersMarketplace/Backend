using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.DataTransferObjects.Order
{
    public class DashboardDto
    {
        public Guid ProducerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Producer Producer { get; set; }
    }

}
