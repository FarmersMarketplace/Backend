using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Account
{
    public class DeleteAccountDto
    {
        public Guid Id { get; set; }
        public Role Role { get; set; }
    }

}
