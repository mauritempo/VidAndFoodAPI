using Application.Interfaces;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

        public Guid UserId
        {
            get
            {
                var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return string.IsNullOrEmpty(idClaim) ? Guid.Empty : Guid.Parse(idClaim);
            }
        }

        public string Email => User?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        public Role Role
        {
            get
            {
                var roleString = User?.FindFirst(ClaimTypes.Role)?.Value;
                if (string.IsNullOrEmpty(roleString)) return Role.User; // Default

                // Convertir string a Enum
                return Enum.TryParse(roleString, out Role role) ? role : Role.User;
            }
        }

        public bool IsInRole(string role)
        {
            return User?.IsInRole(role) ?? false;
        }
    }
}
