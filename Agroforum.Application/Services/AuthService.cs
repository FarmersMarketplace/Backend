using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services
{
    public class AuthService : IAuthService
    {
        public async Task Register(RegisterDto accountDto)
        {
            throw new NotImplementedException();
        }

        public async Task ConfirmEmail(EmailConfirmationDto emailConfirmationDto)
        {
            throw new NotImplementedException();
        }

        public async Task AddPhoneNumber(AddPhoneDto addPhoneDto)
        {
            throw new NotImplementedException();
        }

        public async Task ConfirmPhone(PhoneConfirmationDto phoneConfirmationDto)
        {
            throw new NotImplementedException();
        }

        public async Task<TokenVm> Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }
    }
}
