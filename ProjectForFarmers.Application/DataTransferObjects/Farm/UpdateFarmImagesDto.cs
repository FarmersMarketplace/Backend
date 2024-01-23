using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class UpdateFarmImagesDto
    {
        public Guid FarmId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
