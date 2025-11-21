using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public Task<User?> GetUser(string email, string password);
        public Task<User?> GetByEmailAsync(string email);
    }
}
