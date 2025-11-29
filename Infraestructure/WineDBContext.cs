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




            modelBuilder.Entity<User>()
                .Property(e => e.RoleUser).HasConversion<string>().HasMaxLength(32);

            modelBuilder.Entity<Wine>()
                .Property(e => e.WineType).HasConversion<string>().HasMaxLength(24);

            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(x => x.Email).IsUnique();
                b.Property(x => x.Email).IsRequired().HasMaxLength(256);
                b.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);
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

         
            });

            modelBuilder.Entity<Wine>(b =>
            {
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
                b.HasKey(x => x.UuId);
                b.HasIndex(x => new { x.UserId, x.WineId }).IsUnique();
                b.Property(x => x.Score)
                    .IsRequired();
                b.ToTable(tb => tb.HasCheckConstraint("CK_Rating_Score", "[Score] >= 1 AND [Score] <= 5"));
                b.Property(x => x.Review)
                    .HasMaxLength(2000)
                    .IsRequired(false);
                b.Property(x => x.IsPublic)
                    .HasDefaultValue(true);
                b.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade); // Si se borra el usuario, se borran sus votos
                b.HasOne(r => r.Wine)
                    .WithMany()
                    .HasForeignKey(r => r.WineId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WineUser>(b =>
            {
                b.HasKey(x => x.UuId);

                // ÍNDICE: Un usuario tiene un solo registro de historial por vino
                // (Aquí acumula cuántas veces lo tomó).
                b.HasIndex(x => new { x.UserId, x.WineId }).IsUnique();

                b.Property(x => x.TimesConsumed).HasDefaultValue(1);
                b.Property(x => x.TastingNotes).HasMaxLength(2000);

                // Relaciones
                b.HasOne(wu => wu.User)
                    .WithMany(u => u.WineUsers) // <--- AQUÍ conectamos la colección del Usuario
                    .HasForeignKey(wu => wu.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(wu => wu.Wine)
                    .WithMany()
                    .HasForeignKey(wu => wu.WineId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WineUserCellarItem>(b =>
            {
                b.HasKey(x => x.UuId);
                b.HasIndex(x => new { x.CellarPhysicsId, x.WineId }).IsUnique();
                b.Property(x => x.Quantity).HasDefaultValue(1);
                b.Property(x => x.LocationNote).HasMaxLength(200);
                b.Property(x => x.PurchasePrice).HasColumnType("decimal(18,2)");
                b.Property(x => x.DateAdded).HasDefaultValueSql("GETUTCDATE()");
                // 4. RELACIÓN: CAVA (Padre) <-> ITEM (Hijo)
                b.HasOne(ci => ci.CellarPhysics)
                    .WithMany(cp => cp.Items) 
                    .HasForeignKey(ci => ci.CellarPhysicsId)
                    .OnDelete(DeleteBehavior.Cascade); // Si borro la cava, se borran las botellas.
                // 5. RELACIÓN: VINO (Maestro) <-> ITEM (Referencia)
                b.HasOne(ci => ci.Wine)
                    .WithMany(w => w.CellarItems) // Conecta con la lista en Wine (si la agregaste)
                    .HasForeignKey(ci => ci.WineId)
                    .OnDelete(DeleteBehavior.Restrict); // IMPORTANTE: No permite borrar el vino del catálogo si alguien lo tiene.
                b.ToTable(tb => tb.HasCheckConstraint("CK_CellarItem_Quantity", "[Quantity] > 0"));
            });

            modelBuilder.Entity<CellarPhysics>(b =>
            {
                b.HasKey(x => x.UuId);
                b.Property(x => x.Name).IsRequired().HasMaxLength(100);
                b.Property(x => x.IsActive).HasDefaultValue(true);
                b.Property(x => x.Capacity).IsRequired(false); // Puede ser null
                // Relación: USUARIO -> tiene muchas -> CAVAS
                b.HasOne(cp => cp.User)
                    .WithMany()
                    .HasForeignKey(cp => cp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Relación: CAVA -> tiene muchos -> ITEMS
                b.HasMany(cp => cp.Items)
                    .WithOne(item => item.CellarPhysics)
                    .HasForeignKey(item => item.CellarPhysicsId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
