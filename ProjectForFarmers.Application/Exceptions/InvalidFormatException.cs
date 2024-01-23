using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Exceptions
{
    public class InvalidFormatException : Exception
    {
        public InvalidFormatException(string message) : base(message)
        {
        }
    }

}
