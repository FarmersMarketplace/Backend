using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.DataTransferObjects.Farm
{
    public class GetFarmDto
    {
        public Guid FarmId { get; set; }

        public GetFarmDto(Guid id)
        {
            FarmId = id;
        }
    }
}
