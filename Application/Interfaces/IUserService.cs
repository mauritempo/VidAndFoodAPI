using Application.Models.Request.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(UserCreateDto dto);
        Task<UserDto?> GetByIdAsync(int id);
    }
}
