using Application.Models.Request.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    internal interface IAuthentication
    {
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto request);
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string password);
        string GenerateToken(User user);
        string GenerateRefreshToken();
        DateTime GetTokenExpirationTime();

    }
}
