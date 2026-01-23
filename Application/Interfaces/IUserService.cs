using Application.Models.Request.User;
using Application.Models.Response.User;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        //Task<UserDto> CreateAdminForceAsync(CreateAdminRequest request);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(UserCreateDto dto);
        Task<UserDto?> GetByIdAsync(int id);

        Task<UserProfileDto> GetUserByIdAsync(Guid id);
        Task UpgradeToSommelierAsync();
        Task DownGradeToUserAsync();

        Task<List<UserProfileDto>> GetAllUsersAsync();

        Task DeleteUserAsync(Guid id);

        Task ChangeRoleAsync(Guid userUuId, Role newRole);

    }
}
