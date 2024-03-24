using FarmersMarketplace.Application.DataTransferObjects.Auth;
using FarmersMarketplace.Application.ViewModels.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmersMarketplace.Application.Services
{
    public interface IAuthService
    {
        Task Register(RegisterDto dto);
        Task ConfirmEmail(Guid userId, string email);
        Task<LoginVm> Login(LoginDto dto);
        Task ResetPassword(Guid accountId, string? email, ResetPasswordDto dto);
        Task ForgotPassword(ForgotPasswordDto dto);
        Task<LoginVm> AuthenticateWithGoogle(AuthenticateWithGoogleDto dto);
        Task ConfirmFarmEmail(Guid farmId, string email);
    }
}
