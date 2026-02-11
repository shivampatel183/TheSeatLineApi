using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.AuthServices.Entity;
using TheSeatLineApi.MasterServices.Entity;

namespace TheSeatLineApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<CityEntity> Cities => Set<CityEntity>();

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
