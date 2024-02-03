using Microsoft.AspNetCore.Http;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class UpdateFarmImagesDto
    {
        public Guid FarmId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}