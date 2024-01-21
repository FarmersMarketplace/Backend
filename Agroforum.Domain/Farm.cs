using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Domain
{
    public class Farm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public string? WebsiteUrl { get; set; }
        public bool IsVisibleOnMap { get; set; }
        public List<string> ImagesPaths { get; set; }
        public Guid OwnerId { get; set; }
        public Guid AddressId { get; set; }
    }
}
