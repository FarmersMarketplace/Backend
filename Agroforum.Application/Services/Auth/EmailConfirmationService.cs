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
        private string MessageBody = @"
<!DOCTYPE html>
<html lang=""en"">

<head>
   <meta charset=""UTF-8"">
   <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
   <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
   <title>Registration Confirmation</title>
   <style>
      body {
         font-family: Arial, sans-serif;
         background-color: #f4f4f4;
         margin: 0;
         padding: 0;
      }

      #container {
         max-width: 600px;
         margin: 20px auto;
         background-color: #ffffff;
         padding: 20px;
         border-radius: 5px;
         box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
      }

      h2 {
         color: #333;
      }

      p {
         color: #555;
      }

      a {
         color: #007BFF;
      }
   </style>
</head>

<body>
   <div id=""container"">
      <h2>Registration Confirmation</h2>
      <p>Dear {0} {1},</p>
      <p>Thank you for registering on our [website/service]. We are pleased to welcome you to our community!</p>
      <p><strong>Name:</strong> {0} {1}<br>
         <strong>Email:</strong> {2}<br>

      </p>
      <p>Please confirm your registration by clicking the link below:</p>
      <p><a href=""http://localhost:5173/#/ConfirmEmail/{3}"">Confirm Registration</a></p>
      <p>If you did not register on our [website/service], please ignore this email.</p>
      <p>Thank you for choosing us! If you have any questions or issues, feel free to contact our support team at <a
            href=""mailto:agroforum.support@gmail.com"">agroforum.support@gmail.com</a>.</p>
      <p>Best regards,<br>
         Agroforum</p>
   </div>
</body>

</html>";

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
            MessageBody = MessageBody.Replace("{0}", dto.Name);
            MessageBody = MessageBody.Replace("{1}", dto.Surname);
            MessageBody = MessageBody.Replace("{2}", dto.Email);
            MessageBody = MessageBody.Replace("{3}", token);


            var mailMessage = new MailMessage
            {
                From = FromAddress,
                Subject = "Agroforum Registration Confirmation",
                Body = MessageBody,
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
