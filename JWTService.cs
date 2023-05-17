using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace appDapper
{
    public class JWTService
    {

        private readonly string _secretKey;
        private readonly string _issuer;

        public JWTService(string secretKey, string issuer)
        {
            _secretKey = secretKey;
            _issuer = issuer;
        }

        public string GenerateToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }
    }
}
