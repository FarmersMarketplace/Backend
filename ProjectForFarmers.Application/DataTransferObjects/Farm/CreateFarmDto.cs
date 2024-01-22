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
        public string WebsiteUrl { get; set; }
        public Guid OwnerId { get; set; }
        public bool IsVisibleOnMap { get; set; }
        public List<IFormFile> Images { get; set; }
        public string Region { get; set; }
        public string Settlement { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string PostalCode { get; set; }
        public string Note { get; internal set; }
    }
}
