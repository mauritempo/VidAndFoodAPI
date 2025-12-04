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

        public async Task<UserProfileDto> GetUserByIdAsync(Guid id)
        {
            // Asumo que tienes un _userRepository genérico o específico
            // Si usas BaseRepository, seguramente tienes GetByIdAsync(id)
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"El usuario con ID {id} no existe.");
            }

            return new UserProfileDto
            {
                Id = user.UuId,
                Email = user.Email,
                FullName = user.Email, // Placeholder si no tienes campo nombre
                Role = user.RoleUser.ToString(),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
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
                CreatedAt = DateTime.UtcNow,
                RoleUser = Role.User,
            };

            // Hashear Password
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);


            var addedUser = await _userRepository.AddAsync(user);

            return addedUser.ToDto();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            // Nota: Si tu repositorio usa Guid, cambia 'int id' a 'Guid id'
            var u = await _userRepository.GetByIdAsync(id);

            // CORRECCIÓN: Usamos la extensión .ToDto()
            return u?.ToDto();
        }



        //public async Task<UserDto> CreateAdminForceAsync(CreateAdminRequest request)
        //{
        //    var email = request.Email.Trim();

        //    // 1. Validar que no exista
        //    var existing = await _userRepository.GetByEmailAsync(email);
        //    if (existing != null)
        //        throw new InvalidOperationException($"El usuario {email} ya existe.");

        //    // 2. Crear la entidad forzando el Rol ADMIN
        //    var adminUser = new User
        //    {
        //        Email = email,
        //        FullName = request.FullName,
        //        IsActive = true,
        //        CreatedAt = DateTime.UtcNow,
        //        RoleUser = Role.Admin // <--- AQUÍ ESTÁ LA CLAVE
        //    };

        //    // 3. Hashear password
        //    adminUser.PasswordHash = _hasher.HashPassword(adminUser, request.Password);

        //    // 4. Guardar
        //    var created = await _userRepository.AddAsync(adminUser);

        //    return created.ToDto();
        //}
    }
}