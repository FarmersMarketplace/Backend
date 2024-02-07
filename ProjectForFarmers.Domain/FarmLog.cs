using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class FarmLog
    {
        public Guid Id { get; set; }
        public Guid FarmId { get; set; }
        public string Message { get; set; }
        public List<string> Parameters { get; set; }
        public DateTime CreationDate { get; set; }
    }

}
