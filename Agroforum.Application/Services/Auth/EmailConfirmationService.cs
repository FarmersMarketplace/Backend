using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace Agroforum.Application.Services.Auth
{
    public class EmailConfirmationService
    {
        private SmtpClient SmtpClient { get; set; }
        private MailAddress FromAddress { get; set; }
        private readonly IConfiguration Configuration;
        private string ConfirmationLink => "";
        private int TokenLifetime => 2;

        public EmailConfirmationService(IConfiguration configuration)
        {
            Configuration = configuration;

            FromAddress = new MailAddress("cprog3321@gmail.com", "Agroforum");
            SmtpClient = new SmtpClient();
            SmtpClient.Host = "smtp.gmail.com";
            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.Credentials = new NetworkCredential(FromAddress.Address, "hlaavdzwszaaohmh");
        }
        
        public async Task SendConfirmationEmail(string token, string toEmail)
        {
            string messageBody = $@"{token}";

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

        public async Task SendPasswordResetEmail(string token, string toEmail)
        {
            string messageBody = $@"{token}";

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
    }
}
