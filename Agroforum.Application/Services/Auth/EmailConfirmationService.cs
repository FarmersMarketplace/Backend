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
using Agroforum.Application.DataTransferObjects.Auth;
using System.Net.Http;

namespace Agroforum.Application.Services.Auth
{
    public class EmailConfirmationService
    {
        private SmtpClient SmtpClient { get; set; }
        private MailAddress FromAddress { get; set; }
        private string ConfirmationLink => "";

        public EmailConfirmationService()
        {
            FromAddress = new MailAddress("cprog3321@gmail.com", "Agroforum");
            SmtpClient = new SmtpClient();
            SmtpClient.Host = "smtp.gmail.com";
            SmtpClient.Port = 587;
            SmtpClient.EnableSsl = true;
            SmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpClient.UseDefaultCredentials = false;
            SmtpClient.Credentials = new NetworkCredential(FromAddress.Address, "hlaavdzwszaaohmh");
        }
        
        public async Task SendConfirmationEmail(string token, RegisterDto dto)
        {
            var filePath = Directory.GetParent(Environment.CurrentDirectory) + "\\Agroforum.Application\\EmailTemplates\\verifyEmail.html";
            string messageBody = File.ReadAllText(filePath);
            messageBody = messageBody.Replace("{0}", dto.Name);
            messageBody = messageBody.Replace("{1}", dto.Surname);
            messageBody = messageBody.Replace("{2}", dto.Email);
            messageBody = messageBody.Replace("{3}", token);

            var mailMessage = new MailMessage
            {
                From = FromAddress,
                Subject = "Agroforum Registration Confirmation",
                Body = messageBody,
                IsBodyHtml = true
            };
            mailMessage.To.Add(dto.Email);

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
