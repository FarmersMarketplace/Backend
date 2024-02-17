using ProjectForFarmers.Application.DataTransferObjects.Auth;
using ProjectForFarmers.Application.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectForFarmers.Application.Services
{
    public interface IAuthService
    {
        Task Register(RegisterDto accountDto);
        Task ConfirmEmail(Guid userId, string email);
        Task<LoginVm> Login(LoginDto loginDto);
        Task ResetPassword(Guid accountId, string? email, ResetPasswordDto resetPasswordDto);
        Task ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<LoginVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto authenticateWithGoogleDto);
        Task ConfirmFarmEmail(Guid farmId, string email);
    }
}
