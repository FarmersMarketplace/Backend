using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services
{
    public static class EmailContentBuilder
    {
        public static string ResetPasswordMessageBody(string token)
        {
            return $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>Password Reset Instructions</title>
</head>
<body>
  <p>You are receiving this email because you or someone else has requested a password reset for your account.</p>
  <p>If it was you, please click the link below to reset your password:</p>
  <a href=""http://localhost:5173/#/resetPassword/{token}"">Reset Password</a>
  <p>If you did not request a password reset, please ignore this email.</p>
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
   <style>
      body {{
         font-family: Arial, sans-serif;
         background-color: #f4f4f4;
         margin: 0;
         padding: 0;
      }}

      #container {{
         max-width: 600px;
         margin: 20px auto;
         background-color: #ffffff;
         padding: 20px;
         border-radius: 5px;
         box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
      }}

      h2 {{
         color: #333;
      }}

      p {{
         color: #555;
      }}

      a {{
         color: #007BFF;
      }}
   </style>
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
      <p><a href=""http://localhost:5173/#/ConfirmEmail/{token}"">Confirm Registration</a></p>
      <p>If you did not register on our [website/service], please ignore this email.</p>
      <p>Thank you for choosing us! If you have any questions or issues, feel free to contact our support team at <a
            href=""mailto:agroforum.sup@gmail.com"">agroforum.support@gmail.com</a>.</p>
      <p>Best regards,<br>
         Agroforum</p>
   </div>
</body>

</html>";
        }

    }
}
