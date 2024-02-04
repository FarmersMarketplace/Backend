using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Exceptions
{
    public class DuplicateEmailException : ApplicationException
    {
        public DuplicateEmailException(string message, string userFacingMessage) : base(message, userFacingMessage)
        {
        }
    }

}
