using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.AuthServices.Entity;
using TheSeatLineApi.Entity;
using TheSeatLineApi.MasterServices.Entity;

namespace TheSeatLineApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<CityEntity> Cities => Set<CityEntity>();
        public DbSet<VenueEntity> Venues => Set<VenueEntity>();
        public DbSet<EventEntity> Events => Set<EventEntity>();
        public DbSet<ShowEntity> Shows => Set<ShowEntity>();
        public DbSet<ShowSeatCategoryEntity> ShowSeatCategories => Set<ShowSeatCategoryEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();
        }
    }
}
