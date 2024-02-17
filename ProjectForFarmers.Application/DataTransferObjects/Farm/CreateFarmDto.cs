using Microsoft.AspNetCore.Http;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class CreateFarmDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string? SocialPageUrl { get; set; }
        public Guid OwnerId { get; set; }
        public List<IFormFile>? Images { get; set; }
        public AddressDto Address { get; set; }
        public ScheduleDto Schedule { get; set; }
    }
}
