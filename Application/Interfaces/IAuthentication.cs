using Application.Models.Request.Auth;
using Application.Models.Request.User;
using Application.Models.Response.Auth;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthentication
    {
        Task<AuthResponse?> LoginAsync(LoginRequest request);
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string password);
        string GenerateToken(User user);
        string GenerateRefreshToken();
        DateTime GetTokenExpirationTime();

    }
}
