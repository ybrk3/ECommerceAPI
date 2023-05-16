using ECommerceAPI.Application.Abstractions.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services.Token
{
    public class TokenHandler : ITokenHandler
    {
        //Configuration to be used to reach data in appsettings
        IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Application.DTOs.Token CreateAccessToken(int expiration)
        {
            //Create symethric key
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Create encrypted identity(Signing Credentials)
            SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            //Settings for token to be created
            Application.DTOs.Token token = new Application.DTOs.Token();
            //set expiration time for which value comes as parameter
            token.Expiration = DateTime.UtcNow.AddMinutes(expiration); //it starts on date time now & to be valid during given minutes

            JwtSecurityToken securityToken = new(
                audience: _configuration["Token:Audience"],
                issuer: _configuration["Token:Issuer"],
                expires: token.Expiration,
                notBefore: DateTime.UtcNow, //to be kicked in as soon as created
                signingCredentials: signingCredentials
                );

            //After settings, create token by getting instance from security token creator
            JwtSecurityTokenHandler securityTokenHandler = new();
            token.AccessToken = securityTokenHandler.WriteToken(securityToken);
            return token;
        }
    }
}
