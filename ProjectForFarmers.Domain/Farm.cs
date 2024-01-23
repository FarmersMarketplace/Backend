using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class Farm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string? WebsiteUrl { get; set; }
        public List<string> ImagesNames { get; set; }
        public Guid OwnerId { get; set; }
        public virtual Account Owner { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
        public Guid ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
        

    }
}
