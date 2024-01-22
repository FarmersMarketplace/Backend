namespace ProjectForFarmers.Application.ViewModels.Auth
{
    public class JwtVm
    {
        public string Token { get; set; }

        public JwtVm(string token)
        {
            Token = token;
        }
    }
}
