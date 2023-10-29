namespace Agroforum.Application.DataTransferObjects.Auth
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword{ get; set; }
        public bool IsFarmer { get; set; }
    }
}
