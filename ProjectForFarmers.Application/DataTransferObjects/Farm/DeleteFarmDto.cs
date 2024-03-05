using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Farm
{
    public class DeleteFarmDto
    {
        public Guid FarmId { get; set; }
        public Guid? OwnerId { get; set; }
    }
}
