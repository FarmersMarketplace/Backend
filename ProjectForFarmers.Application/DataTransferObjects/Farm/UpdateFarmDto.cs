﻿using Microsoft.AspNetCore.Http;
using FarmersMarketplace.Domain;

namespace FarmersMarketplace.Application.DataTransferObjects.Farm
{
    public class UpdateFarmDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactPhone { get; set; }
        public string? AdditionalPhone { get; set; }
        public string? FirstSocialPageUrl { get; set; }
        public string? SecondSocialPageUrl { get; set; }
        public AddressDto Address { get; set; }
        public ScheduleDto Schedule { get; set; }
        public List<IFormFile> Images { get; set; }
        public List<ReceivingMethod> ReceivingMethods { get; set; }
    }

}
