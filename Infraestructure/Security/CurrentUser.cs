using Application.Interfaces;        
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;          
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _http;
        public CurrentUser(IHttpContextAccessor http) => _http = http;

        public string? UserId =>
            _http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? _http.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        public Role? Role
        {
            get
            {
                var value = _http.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
                return Enum.TryParse<Role>(value, out var r) ? r : null;
            }
        }
    }
}
