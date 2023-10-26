using Agroforum.Application.DataTransferObjects.Auth;
using Agroforum.Application.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services
{
    public interface IAuthService
    {
        Task<RegisterVm> Register(RegisterDto accountDto);

        Task ConfirmEmail(EmailConfirmationDto emailConfirmationDto);

        Task AddPhoneNumber(AddPhoneDto addPhoneDto);

        Task ConfirmPhone(PhoneConfirmationDto phoneConfirmationDto);

        Task<TokenVm> Login(LoginDto loginDto);
    }
}
