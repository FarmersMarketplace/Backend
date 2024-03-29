using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Domain
{
    public class Feedback
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Comment { get; set; }
        public float Rating { get; set; }
    }

}
