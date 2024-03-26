using FarmersMarketplace.Application.DataTransferObjects.Farm;
using FarmersMarketplace.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class UpdateSellerDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public string? AdditionalPhone { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public List<IFormFile>? Images { get; set; }
        public AddressDto? Address { get; set; }
        public virtual ScheduleDto? Schedule { get; set; }
        public string? FirstSocialPageUrl { get; set; }
        public string? SecondSocialPageUrl { get; set; }
        public List<ReceivingMethod>? ReceivingMethods { get; set; }
    }

}
