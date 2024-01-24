using AutoMapper;
using ProjectForFarmers.Application.Interfaces;
using ProjectForFarmers.Domain;

namespace ProjectForFarmers.Application.DataTransferObjects.Auth
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword{ get; set; }
        public Role Role { get; set; }

        
    }
}
