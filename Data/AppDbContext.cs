using Investment_Tracker.Entities;
using Microsoft.EntityFrameworkCore;

namespace Investment_Tracker.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Asset> Assets => Set<Asset>();
        public DbSet<Investment> Investments => Set<Investment>();
        public DbSet<PriceHistory> PriceHistory => Set<PriceHistory>(); 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Asset>()
                .HasIndex(a => a.code)
                .IsUnique();

            modelBuilder.Entity<Asset>()
                .Property(a => a.code)
                .HasMaxLength(20);

            modelBuilder.Entity<Asset>()
                .Property(a => a.category)
                .HasMaxLength(20);

            modelBuilder.Entity<Asset>()                       
                .Property(a => a.lastPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Investment>()
                .Property(i => i.quantity)
                .HasPrecision(18, 8);

            modelBuilder.Entity<Investment>()
                .Property(i => i.buyPrice)
                .HasPrecision(18, 8);

            modelBuilder.Entity<PriceHistory>()
                .HasIndex(p => new { p.assetId, p.date })
                .IsUnique();

            modelBuilder.Entity<PriceHistory>()
                .Property(p => p.close)
                .HasPrecision(18, 8);
        }
    }
}
