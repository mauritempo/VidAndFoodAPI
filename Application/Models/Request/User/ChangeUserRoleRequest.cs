using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request.User
{
    public class ChangeUserRoleRequest
    {
        public Guid UserUuId { get; set; }   // o UserId, según tu convención
        public Role NewRole { get; set; }
    }
}
