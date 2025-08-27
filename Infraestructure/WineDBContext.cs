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
        public DbSet<Wine> Wines { get; set; }
        public DbSet<WineUser> WineUsers { get; set; }
        public DbSet<WineUserCellarItem> WineUserCellarItems { get; set; }
        public DbSet<Grape> Grapes { get; set; }
        public DbSet<WineGrapeVariety> WineGrapeVarieties { get; set; }
        public DbSet<CellarPhysics> CellarPhysics { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public WineDBContext() { }
        public WineDBContext(DbContextOptions<WineDBContext> options) : base(options)
        {

        }
    }
}
