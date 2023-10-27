using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace Agroforum.Application.Services.Auth
{
    public class EmailConfirmationService
    {
        private SmtpClient SmtpClient { get; set; }
        private MailAddress FromAddress { get; set; }
        private string ConfirmationLink => "";
        private int TokenLifetime => 2;
        private string SecretKey => "";

        public EmailConfirmationService()
        {
            FromAddress = new MailAddress("cprog3321@gmail.com", "MusicBlendHub.Identity");

            SmtpClient = new SmtpClient();
            SmtpClient.Host = "smtp.gmail.com";
            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.Credentials = new NetworkCredential(FromAddress.Address, "hlaavdzwszaaohmh");
        }
        
        public async Task SendEmailAsync(Guid userId, string toEmail)
        {
            string token = GenerateToken(userId, toEmail);

            string messageBody = $@"{ConfirmationLink + "/" + token}";

            var mailMessage = new MailMessage
            {
                From = FromAddress,
                Subject = "Agroforum Registration Confirmation",
                Body = messageBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(toEmail);

            SmtpClient.Send(mailMessage);
        }

        private string GenerateToken(Guid userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.Now.AddHours(TokenLifetime),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
