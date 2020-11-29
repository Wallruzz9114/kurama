using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;

namespace Core.Implementations
{
    public class JWTGenerator : IJWTGenerator
    {
        private readonly SymmetricSecurityKey _key;

        public JWTGenerator(IConfiguration configuration) =>
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

        public string CreateToken(AppUser appUser)
        {
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, appUser.UserName) };
            var symmetricSecurityKey = _key;
            var signInCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signInCredentials,
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}