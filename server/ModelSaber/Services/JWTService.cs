using System;
using System.Text;
using System.Security.Claims;
using ModelSaber.Models.User;
using ModelSaber.Models.Settings;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace ModelSaber.Services
{
    public class JWTService
    {
        private readonly string _key;
        private readonly string _issuer;

        public JWTService(IJWTSettings settings)
        {
            _key = settings.Key;
            _issuer = settings.Issuer;
        }

        public string GenerateUserToken(User user, float timeInHours = 30240f)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _issuer,
                claims,
                expires: DateTime.UtcNow.AddHours(timeInHours),
                signingCredentials: credentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedToken;
        }
    }
}