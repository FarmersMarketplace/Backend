using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services
{
    public static class EmailContentBuilder
    {
        public static string ResetPasswordMessageBody(string name, string surname, string token)
        {
            return $@"
<!DOCTYPE html>
<html lang=""en"">

<head>
   <meta charset=""UTF-8"">
   <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
   <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
   <title>Password Reset Confirmation</title>
</head>

<body>
   <div id=""container"">
      <h2>Password Reset Confirmation</h2>
      <p>Dear {name} {surname},</p>
      <p>We have received a request to reset your password for your account on our website.</p>
      <p>Please confirm the password reset by clicking the link below:</p>
      <p><a href=""http://localhost:8080/resetpassword/{token}"">Reset Password</a></p>
      <p>If you did not request a password reset, please ignore this email.</p>
      <p>For security reasons, the link will expire in [insert time here].</p>
      <p>If you have any questions or need further assistance, feel free to contact our support team at <a
            href=""mailto:[serviceEmail]"">[serviceEmail]</a>.</p>
      <p>Best regards,<br>
         [Your Company/Organization Name]</p>
   </div>
</body>

</html>";
        }

        public static string ConfirmationMessageBody(string name, string surname, string email, string token)
        {
            return $@"
<!DOCTYPE html>
<html lang=""en"">

<head>
   <meta charset=""UTF-8"">
   <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
   <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
   <title>Registration Confirmation</title>
</head>

<body>
   <div id=""container"">
      <h2>Registration Confirmation</h2>
      <p>Dear {name} {surname},</p>
      <p>Thank you for registering on our website. We are pleased to welcome you to our community!</p>
      <p><strong>Name:</strong> {name} {surname}<br>
         <strong>Email:</strong> {email}<br>

      </p>
      <p>Please confirm your registration by clicking the link below:</p>
      <p><a href=""http://localhost:8080/confirmemail/{token}"">Confirm Registration</a></p>
      <p>If you did not register on our [website/service], please ignore this email.</p>
      <p>Thank you for choosing us! If you have any questions or issues, feel free to contact our support team at <a
            href=""mailto:[serviceEmail]"">[serviceEmail]</a>.</p>
      <p>Best regards,<br>
         Agroforum</p>
   </div>
</body>

</html>";
        }

    }
}
