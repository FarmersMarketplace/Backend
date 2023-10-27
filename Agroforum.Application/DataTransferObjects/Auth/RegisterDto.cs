﻿namespace Agroforum.Application.DataTransferObjects.Auth
{
    public class RegisterDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsFarmer { get; set; }
        public DateTime DispatchDate { get; set; }
    }
}
