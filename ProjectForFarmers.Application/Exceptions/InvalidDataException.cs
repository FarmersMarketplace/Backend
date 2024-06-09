using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Exceptions
{
    public class InvalidDataException : ApplicationException
    {

        public InvalidDataException(string message, string userFacingMessage) : base(message, userFacingMessage)
        {
        }

        public InvalidDataException() : base()
        {
        }

        public InvalidDataException(string message, string userFacingMessage, string? environment, string? action) : base(message, userFacingMessage, environment, action)
        {
        }
    }

}
