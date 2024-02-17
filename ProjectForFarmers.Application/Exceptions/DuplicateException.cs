using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Exceptions
{
    public class DuplicateException : ApplicationException
    {
        public DuplicateException(string message, string userFacingMessage) : base(message, userFacingMessage)
        {
        }

        public DuplicateException() : base()
        {
            
        }

        public DuplicateException(string message, string userFacingMessage, string? environment, string? action) : base(message, userFacingMessage, environment, action)
        {
        }
    }

}
