using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.AuthServices.Entity;
using TheSeatLineApi.BookingServices.Entity;
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
        public DbSet<BookingEntity> Bookings => Set<BookingEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            // Booking entity configuration
            modelBuilder.Entity<BookingEntity>()
                .HasIndex(b => b.UserId);

            modelBuilder.Entity<BookingEntity>()
                .HasIndex(b => b.ShowId);

            modelBuilder.Entity<BookingEntity>()
                .HasIndex(b => b.BookingStatus);

            modelBuilder.Entity<BookingEntity>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingEntity>()
                .HasOne(b => b.Show)
                .WithMany()
                .HasForeignKey(b => b.ShowId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BookingEntity>()
                .HasOne(b => b.ShowSeatCategory)
                .WithMany()
                .HasForeignKey(b => b.ShowSeatCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
