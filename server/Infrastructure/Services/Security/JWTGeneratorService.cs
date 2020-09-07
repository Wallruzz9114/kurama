using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Interfaces.Security;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.Security
{
    public class JWTGeneratorService : IJWTGeneratorService
    {
        private readonly SymmetricSecurityKey _symmetricSecurityKey;

        public JWTGeneratorService(IConfiguration configuration) =>
            _symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokenkey"]));

        public string CreateToken(AppUser appUser)
        {
            var claims = new List<Claim> { new Claim(JwtRegisteredClaimNames.NameId, appUser.UserName) };
            var signingCredentials = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtSecurityTokenHandler.WriteToken(securityToken);
        }
    }
}