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
        Task Register(RegisterDto accountDto);
        Task ConfirmEmail(Guid userId, string email);
        Task<JwtVm> Login(LoginDto loginDto);
        Task ResetPassword(Guid accountId, string? email, ResetPasswordDto resetPasswordDto);
        Task ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<JwtVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto authenticateWithGoogleDto);
    }
}
