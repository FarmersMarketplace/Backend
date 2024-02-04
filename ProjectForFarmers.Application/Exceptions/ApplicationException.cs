using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public string UserFacingMessage { get; set; }

        public ApplicationException(string message, string userFacingMessage) : base(message) 
        {  
            UserFacingMessage = userFacingMessage;
        }

    }

}
