using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly WineDBContext _dbContext;
        public UserRepository(WineDBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
