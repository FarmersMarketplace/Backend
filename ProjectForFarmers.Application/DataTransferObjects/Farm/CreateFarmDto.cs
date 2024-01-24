using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class CreateFarmDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string WebsiteUrl { get; set; }
        public Guid OwnerId { get; set; }
        public List<IFormFile> Images { get; set; }
        public AddressDto Address { get; set; }
        public ScheduleDto Schedule { get; set; }
    }
}
