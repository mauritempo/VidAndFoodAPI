using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class WineDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public WineDBContext() { }
        public WineDBContext(DbContextOptions<WineDBContext> options) : base(options)
        {

        }
    }
}
