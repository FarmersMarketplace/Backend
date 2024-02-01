using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class UpdateFarmDto
    {
        public Guid FarmId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string? SocialPageUrl { get; internal set; }
        public AddressDto Address { get; set; }
        public ScheduleDto Schedule { get; set; }
    }
}
