using Application.Interfaces;
using Application.mapper;
using Application.Models.Request.Auth;  // <--- Tus nuevos DTOs de Request
using Application.Models.Response.Auth; // <--- Tus nuevos DTOs de Response
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class AuthService : IAuthentication
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> hasher,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _configuration = configuration;
        }

        public async Task<AuthResponse?> LoginAsync(LoginRequest request) // <--- Nombre limpio
        {
            // 1. Buscar usuario
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null) return null;

            // 2. Verificar contraseña
            if (!VerifyPassword(user, request.Password))
                return null;

            // 3. Generar respuesta usando el DTO limpio y el Mapeador
            return new AuthResponse
            {
                Token = GenerateToken(user),
                RefreshToken = GenerateRefreshToken(),
                ExpiresAt = GetTokenExpirationTime(),
                User = user.ToDto() // <--- ¡Mapeo reutilizable! (UserMappingExtensions)
            };
        }

        public string HashPassword(User user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string password)
        {
            return _hasher.VerifyHashedPassword(user, user.PasswordHash, password)
                != PasswordVerificationResult.Failed;
        }

        public string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                // USAMOS UUID para ser consistentes con el UserDto que devolvemos
                new(ClaimTypes.NameIdentifier, user.UuId.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.RoleUser.ToString())
            };

            if (!string.IsNullOrEmpty(user.FullName))
                claims.Add(new Claim(ClaimTypes.Name, user.FullName));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: GetTokenExpirationTime(),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public DateTime GetTokenExpirationTime()
        {
            return DateTime.UtcNow.AddHours(1);
        }
    }
}