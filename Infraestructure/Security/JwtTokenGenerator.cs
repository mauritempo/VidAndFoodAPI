// Infrastructure/Security/JwtTokenGenerator.cs
using Application.Interfaces;              // o Application.Interfaces.Security si movés ahí la interfaz
using Application.Options;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Security
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _opts;
        private DateTime _lastExpiryUtc;

        public JwtTokenGenerator(IOptions<JwtOptions> opts)
        {
            _opts = opts.Value;
        }

        public string Generate(User user)
        {
            if (string.IsNullOrWhiteSpace(_opts.Secret))
                throw new InvalidOperationException("JWT Secret no configurado.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.UuId.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, user.RoleUser.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            _lastExpiryUtc = now.AddMinutes(_opts.ExpMinutes);

            var token = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                notBefore: now,
                expires: _lastExpiryUtc,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime GetExpiryUtc() => _lastExpiryUtc;
    }
}
