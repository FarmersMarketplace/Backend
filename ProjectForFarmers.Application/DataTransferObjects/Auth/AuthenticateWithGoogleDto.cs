using FarmersMarketplace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.DataTransferObjects.Auth
{
    public class AuthenticateWithGoogleDto
    {
        public string GoogleIdToken { get; set; }
    }
}
