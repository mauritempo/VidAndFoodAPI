using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        string Email { get; }
        Role Role { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}
