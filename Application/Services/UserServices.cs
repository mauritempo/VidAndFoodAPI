using Application.Interfaces;
using Application.mapper;
using Application.Models.Request.User;
using Application.Models.Response.User; // <--- IMPORTANTE: DTO de salida
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;

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

            // CORRECCIÓN: Usamos la extensión .ToDto()
            return u?.ToDto();
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            // Validaciones
            if (dto is null) throw new ArgumentNullException(nameof(dto));
            if (string.IsNullOrWhiteSpace(dto.Email)) throw new ArgumentException("Email requerido.");
            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres.");

            var email = dto.Email.Trim();

            // Verificar duplicados
            var existing = await _userRepository.GetByEmailAsync(email);
            if (existing != null)
                throw new InvalidOperationException("El email ya está en uso.");

            // Crear Entidad
            var user = new User
            {
                Email = email,
                FullName = dto.FullName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            // Hashear Password
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            // Asignar Rol (con seguridad)
            var callerRole = _current.Role ?? Role.User;
            if (dto.Role.HasValue && dto.Role.Value != Role.User && callerRole != Role.Admin)
                throw new UnauthorizedAccessException("Solo un Admin puede asignar roles elevados.");

            user.RoleUser = dto.Role ?? Role.User;

            // Guardar en BD
            var addedUser = await _userRepository.AddAsync(user);

            // CORRECCIÓN: Retornamos la entidad guardada convertida a DTO
            // NO retornes 'dto' porque ese tiene la password en texto plano.
            return addedUser.ToDto();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            // Nota: Si tu repositorio usa Guid, cambia 'int id' a 'Guid id'
            var u = await _userRepository.GetByIdAsync(id);

            // CORRECCIÓN: Usamos la extensión .ToDto()
            return u?.ToDto();
        }
    }
}