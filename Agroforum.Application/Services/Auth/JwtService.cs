﻿using Agroforum.Application.ViewModels.Auth;
using Agroforum.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Agroforum.Application.Services.Auth
{
    public class JwtService
    {
        private readonly IConfiguration Configuration;

        public JwtService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<JwtVm> Authenticate(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Configuration["Auth:Secret"]);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
            };

            if (account.Roles != null && account.Roles.Any())
            {
                foreach (var role in account.Roles)
                    claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var notBefore = DateTime.UtcNow;
            var expires = notBefore.AddHours(int.Parse(Configuration["Auth:TokenLifetime"]));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Configuration["Auth:Issuer"],
                Audience = Configuration["Auth:Audience"],
                Expires = expires,
                NotBefore = notBefore,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtVm(tokenHandler.WriteToken(token));
        }

        public async Task<string> EmailConfirmationToken(Guid id, string email)
        {
            const int tokenLifetime = 2;
            var notBefore = DateTime.UtcNow;
            var expires = notBefore.AddHours(tokenLifetime);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Auth:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = Configuration["Auth:Issuer"],
                Audience = Configuration["Auth:Audience"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Email, email)
                }),
                NotBefore = notBefore,
                Expires = expires,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}