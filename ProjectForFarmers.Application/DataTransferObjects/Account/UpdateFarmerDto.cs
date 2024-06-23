using FarmersMarketplace.Domain.Accounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class UpdateFarmerDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? Phone { get; set; }
        public string? AdditionalPhone { get; set; }
        public string? Password { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public IFormFile? Avatar { get; set; }
        public AddressDto? Address { get; set; }
    }

}
