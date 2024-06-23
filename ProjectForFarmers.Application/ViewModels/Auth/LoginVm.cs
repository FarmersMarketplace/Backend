using FarmersMarketplace.Domain.Accounts;

namespace FarmersMarketplace.Application.ViewModels.Auth
{
    public class LoginVm
    {
        public string Token { get; set; }
        public Role Role { get; set; }
        public Guid AccountId { get; set; }

        public LoginVm(string token, Role role, Guid accountId)
        {
            Token = token;
            Role = role;
            AccountId = accountId;
        }

        public LoginVm()
        {
            
        }
    }
}
