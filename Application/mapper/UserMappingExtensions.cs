using Application.Models.Request.User;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.mapper
{
    public static class UserMappingExtensions
    {
        public static UserDto ToDto(this User entity)
        {
            if (entity == null) return null;

            return new UserDto
            {
                Id = entity.UuId, // Asegúrate que tu User tenga Id int o Guid
                Email = entity.Email,
                FullName = entity.FullName,
                Role = entity.RoleUser, // Asumiendo que en BD se llama RoleUser
                IsActive = entity.IsActive
            };
        }
    }
}
