using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        // Symmetric key is acceptable since the private key on the server doesnt need a 
        // client key to function (Asymentric would be used for that)
        private readonly SymmetricSecurityKey _key;
        
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(AppUser user)
        {
            // Add a list of claims to create the token from
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            // Allow signing to use a secure algorhithm
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // CreateToken a descriptor to define things that will be in our token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            // Create a handler for the security tokens
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create the token in memory
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // And finally return the written token to the caller
            return tokenHandler.WriteToken(token);
        }
    }
}