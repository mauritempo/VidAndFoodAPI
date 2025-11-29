using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Response.User
{
    public class UserDto
    {
        public Guid Id { get; set; } // O Guid, segun tu entidad User
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool IsActive { get; set; }
    }
}
