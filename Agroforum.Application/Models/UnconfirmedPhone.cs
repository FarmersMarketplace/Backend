using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Models
{
    public class UnconfirmedPhone
    {
        public Guid AccountId { get; set; }
        public string Number { get; set; }
        public string Code { get; set; }
        public DateTime Deadline { get; set; }
    }
}
