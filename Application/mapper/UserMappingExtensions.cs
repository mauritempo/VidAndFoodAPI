using Application.Models.Request.User;
using Application.Models.Response.User;
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
                // Mapeamos el UUID de la base de datos al Id del DTO
                Id = entity.UuId,

                Email = entity.Email,
                FullName = entity.FullName,
                Role = entity.RoleUser,
                IsActive = entity.IsActive

                // Si agregaste CreatedAt al UserDto, agrégalo aquí también:
                // CreatedAt = entity.CreatedAt
            };
        }


    }
}