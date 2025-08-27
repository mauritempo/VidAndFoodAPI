﻿using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly WineDBContext _dbContext;
        public UserRepository(WineDBContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<User?> GetUser(string email, string password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
        }
    }
}
