using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            // 1. ENUMS COMO STRING
            modelBuilder.Entity<User>()
                .Property(e => e.RoleUser)
                .HasConversion<string>()
                .HasMaxLength(32);

            modelBuilder.Entity<Wine>()
                .Property(e => e.WineType)
                .HasConversion<string>()
                .HasMaxLength(24);

            // 2. CONFIGURACIÓN BÁSICA DE ENTIDADES

            // USER
            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);
                b.Property(x => x.IsActive).HasDefaultValue(true);

            });

            // WINE
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

                // Relación Wine -> CellarItems
                b.HasMany(w => w.CellarItems)
                    .WithOne(ci => ci.Wine)
                    .HasForeignKey(ci => ci.WineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // GRAPE
            modelBuilder.Entity<Grape>(b =>
            {
                b.Property(x => x.Name).IsRequired().HasMaxLength(120);
                b.HasIndex(x => x.Name).IsUnique();
            });

            // 3. TABLAS INTERMEDIAS Y RELACIONES COMPLEJAS

            // WINE-GRAPE VARIETY
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

                // Aseguramos nombre de columna + constraint Postgres-friendly
                b.Property(x => x.Percentage)
                    .HasColumnName("percentage");

                b.ToTable(t => t.HasCheckConstraint(
                    "CK_WineGrapeVariety_Percentage",
                    "percentage IS NULL OR (percentage >= 0 AND percentage <= 100)"
                ));
            });

            // WINE FAVORITE
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

            // RATING
            modelBuilder.Entity<Rating>(b =>
            {
                b.HasKey(x => x.UuId);

                b.HasIndex(x => new { x.UserUuId, x.WineUuId }).IsUnique();

                b.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserUuId);

                b.HasOne(r => r.Wine)
                    .WithMany()
                    .HasForeignKey(r => r.WineUuId);
            });


            modelBuilder.Entity<WineUser>(b =>
            {
                b.HasKey(x => x.UuId);

                // Un usuario tiene un solo registro de historial por vino
                b.HasIndex(x => new { x.UserId, x.WineId }).IsUnique();

                b.Property(x => x.TimesConsumed).HasDefaultValue(1);
                b.Property(x => x.TastingNotes).HasMaxLength(2000);

                b.HasOne(wu => wu.Wine)
                    .WithMany()
                    .HasForeignKey(wu => wu.WineId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(wu => wu.User)
                    .WithMany(u => u.WineUsers) 
                    .HasForeignKey(wu => wu.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 
            });

            modelBuilder.Entity<WineUserCellarItem>(b =>
            {
                b.HasKey(x => x.UuId);

                b.HasIndex(x => new { x.CellarPhysicsId, x.WineId }).IsUnique();

                b.Property(x => x.Quantity)
                    .HasDefaultValue(1)
                    .HasColumnName("quantity");

                b.Property(x => x.LocationNote)
                    .HasMaxLength(200);

                // numeric(18,2) en Postgres
                b.Property(x => x.PurchasePrice)
                    .HasColumnType("numeric(18,2)");

                // Fecha en UTC
                b.Property(x => x.DateAdded)
                    .HasDefaultValueSql("NOW() AT TIME ZONE 'UTC'");

                b.HasOne(ci => ci.CellarPhysics)
                    .WithMany(cp => cp.Items)
                    .HasForeignKey(ci => ci.CellarPhysicsId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Constraint de cantidad > 0
                b.ToTable(tb => tb.HasCheckConstraint(
                    "CK_CellarItem_Quantity",
                    "quantity > 0"
                ));
            });

            // CELLAR PHYSICS (CAVAS FÍSICAS DEL USUARIO)
            modelBuilder.Entity<CellarPhysics>(b =>
            {
                b.HasKey(x => x.UuId);

                b.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property(x => x.IsActive)
                    .HasDefaultValue(true);

                b.Property(x => x.Capacity)
                    .IsRequired(false);

                b.HasOne(cp => cp.User)
                    .WithMany()
                    .HasForeignKey(cp => cp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
