using ProjectForFarmers.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.ViewModels.Farm
{
    public class FarmVm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string OwnerName { get; set; }
        public Address Address { get; set; }
        public string? SocialPageUrl { get; set; }
        public List<string> ImagesNames { get; set; }
    }
}
