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
        public DbSet<WineFavorite> WineFavorites { get; set; }

        public WineDBContext(DbContextOptions<WineDBContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

           

            modelBuilder.Entity<User>()
                .Property(e => e.RoleUser).HasConversion<string>().HasMaxLength(32);

            modelBuilder.Entity<Wine>()
                .Property(e => e.WineType).HasConversion<string>().HasMaxLength(24);

            modelBuilder.Entity<WineUser>()
                .Property(e => e.TypeCellar).HasConversion<string>().HasMaxLength(16);

            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.Password).IsRequired().HasMaxLength(256);
                b.Property(x => x.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Wine>(b =>
            {
                b.Property(x => x.Name).IsRequired().HasMaxLength(160);
                b.Property(x => x.WineryName).HasMaxLength(160);
                b.Property(x => x.RegionName).HasMaxLength(160);
                b.Property(x => x.CountryName).HasMaxLength(160);
                b.Property(x => x.LabelImageUrl).HasMaxLength(512);
                b.Property(x => x.TastingNotes).HasMaxLength(2048);
                b.Property(x => x.Aroma).HasMaxLength(1024);
                b.HasIndex(x => new { x.Name, x.VintageYear });
                b.Property(x => x.IsActive).HasDefaultValue(true);
            });

            modelBuilder.Entity<Grape>(b =>
            {
                b.Property(x => x.Name).IsRequired().HasMaxLength(120);
                b.HasIndex(x => x.Name).IsUnique();
            });

            // Relaciones
            
            modelBuilder.Entity<User>(b =>
            {
                b.HasMany(u => u.WineUsers)
                 .WithOne(wu => wu.User)
                 .HasForeignKey(wu => wu.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(u => u.Ratings)
                 .WithOne(r => r.User)
                 .HasForeignKey(r => r.UserId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Wine>(b =>
            {
                b.HasMany(w => w.Ratings)
                 .WithOne(r => r.Wine)
                 .HasForeignKey(r => r.WineId)
                 .OnDelete(DeleteBehavior.Restrict);

                b.HasMany(w => w.CellarItems)
                 .WithOne(ci => ci.Wine)
                 .HasForeignKey(ci => ci.WineId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WineGrapeVariety>(b =>
            {
                b.HasKey(x => new { x.WineId, x.GrapeId });

                b.HasOne(wgv => wgv.Wine)
                    .WithMany(w => w.WineGrapeVarieties)
                    .HasForeignKey(wgv => wgv.WineId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(wgv => wgv.Grape)
                    .WithMany(g => g.WineGrapeVarieties) 
                    .HasForeignKey(wgv => wgv.GrapeId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.ToTable(tb =>
                    tb.HasCheckConstraint("CK_WineGrapeVariety_Percentage",
                        "([Percentage] IS NULL) OR ([Percentage] >= 0 AND [Percentage] <= 100)"));
            });

            modelBuilder.Entity<WineFavorite>(b =>
            {
                b.HasKey(x => new { x.UserId, x.WineId });

                b.HasOne(wf => wf.User)
                    .WithMany(u => u.Favorites)
                    .HasForeignKey(wf => wf.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(wf => wf.Wine)
                    .WithMany(w => w.FavoritedByUsers)
                    .HasForeignKey(wf => wf.WineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Rating>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => new { x.UserId, x.WineId }).IsUnique();

                b.Property(x => x.Rate).IsRequired();
                b.ToTable(tb =>
                    tb.HasCheckConstraint("CK_Rating_Rate", "[Rate] >= 0 AND [Rate] <= 5"));

                b.Property(x => x.IsPublic).HasDefaultValue(false);
            });

            modelBuilder.Entity<WineUser>(b =>
            {
                b.HasKey(x => x.Id);

                b.HasMany(wu => wu.CellarItems)
                 .WithOne(ci => ci.WineUser)
                 .HasForeignKey(ci => ci.WineUserId)
                 .OnDelete(DeleteBehavior.Cascade);

                b.Property(x => x.TastingNotes).HasMaxLength(2048);
                b.Property(x => x.Opinion).HasMaxLength(1024);
                b.Property(x => x.isCellarActive).HasDefaultValue(false);
            });

            modelBuilder.Entity<WineUserCellarItem>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => new { x.WineUserId, x.WineId }).IsUnique();

                b.Property(x => x.Quantity).HasDefaultValue(1);
                b.Property(x => x.LocationNote).HasMaxLength(160);

                b.ToTable(tb =>
                    tb.HasCheckConstraint("CK_WineUserCellarItem_Quantity", "[Quantity] > 0"));
            });

            modelBuilder.Entity<CellarPhysics>(b =>
            {
                b.HasKey(x => x.WineUserId); 

                b.HasOne(cp => cp.WineUser)
                    .WithOne(wu => wu.CellarPhysics) 
                    .HasForeignKey<CellarPhysics>(cp => cp.WineUserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.Property(x => x.Name).IsRequired().HasMaxLength(120);
                b.Property(x => x.IsActive).HasDefaultValue(true);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
    }
