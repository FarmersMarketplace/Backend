using FarmersMarketplace.Domain.Accounts;

namespace FarmersMarketplace.Application.DataTransferObjects.Auth
{
    public class AuthenticateWithGoogleDto
    {
        public string GoogleIdToken { get; set; }
        public Role Role { get; set; }
    }
}
