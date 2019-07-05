using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VayesJwt.Dtos;
using VayesJwt.Models;

namespace VayesJwt.Services
{
    public class AccountService
    {
        private readonly IEnumerable<User> _users = new List<User>
        {
            new User { Id = 1, Username = "vayes1", Password= "vayes1", Role = "Admin"},
            new User { Id = 2, Username = "vayes2", Password= "vayes2", Role = "Employee"},
            new User { Id = 3, Username = "vayes3", Password= "vayes3", Role = "Guest"},
        };

        private readonly IConfiguration _configuration;

        public AccountService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Login(LoginDto loginDto)
        {
             var user = _users.Where(x => x.Username == loginDto.Username && loginDto.Password == x.Password).SingleOrDefault();

            if(user == null)
            {
                return null;
            }

            var signingKey = Convert.FromBase64String(_configuration["Jwt:SigningSecret"]);
            var expiryDuration = int.Parse(_configuration["Jwt:ExpiryDuration"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "www.vayes.com.tr",
                Audience = "www.vayes.com.tr",
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new List<Claim> {
                        new Claim("userid", user.Id.ToString()),
                        new Claim("role", user.Role)
                    }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }
    }
}
