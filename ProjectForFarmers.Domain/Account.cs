using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Domain
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }
}
