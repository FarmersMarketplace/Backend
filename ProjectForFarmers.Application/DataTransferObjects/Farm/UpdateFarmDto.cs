using Microsoft.AspNetCore.Http;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.DataTransferObjects.Farm
{
    public class UpdateFarmDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string? SocialPageUrl { get; set; }
        public AddressDto Address { get; set; }
        public ScheduleDto Schedule { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<ReceivingMethod> ReceivingMethods { get; set; }
    }

}
