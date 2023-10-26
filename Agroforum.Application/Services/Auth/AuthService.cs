using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.Interfaces;
using Agroforum.Application.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private IAgroforumDbContext DbContext { get; set; }

        public AuthService(IAgroforumDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<RegisterVm> Register(RegisterDto accountDto)
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
