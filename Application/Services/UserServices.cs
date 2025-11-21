using Application.Interfaces;
using Application.Models.Request.User;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _hasher;   
        private readonly ICurrentUser _current;

        public UserServices(
            IUserRepository userRepository,
            IPasswordHasher<User> hasher,
            ICurrentUser current)
        {
            _userRepository = userRepository;
            _hasher = hasher;
            _current = current;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            var u = await _userRepository.GetByEmailAsync(email.Trim());
            return u is null ? null : Map(u);
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email requerido.");
            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

            var email = dto.Email.Trim();

            var existing = await _userRepository.GetByEmailAsync(email);
            if (existing != null)
                throw new InvalidOperationException("El email ya está en uso.");

            var user = new User
            {
                Email = email,
                FullName = dto.FullName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };


            user.PasswordHash = _hasher.HashPassword(user, dto.Password);


            var callerRole = _current.Role ?? Role.User;
            if (dto.Role.HasValue && dto.Role.Value != Role.User && callerRole != Role.Admin)
                throw new UnauthorizedAccessException("Solo un Admin puede asignar roles elevados.");

            user.RoleUser = dto.Role ?? Role.User; 

            var added = await _userRepository.AddAsync(user);
            return dto
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            return u is null ? null : Map(u);
        }
        

    }
}
