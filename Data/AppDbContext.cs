using Microsoft.EntityFrameworkCore;

namespace TheSeatLineApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Auth
        public DbSet<User> Users => Set<User>();

        // Master
        public DbSet<City> Cities => Set<City>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<VenueOperatingHours> VenueOperatingHours => Set<VenueOperatingHours>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Seat> Seats => Set<Seat>();

        // Booking
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();
        public DbSet<Coupon> Coupons => Set<Coupon>();

        // Payment
        public DbSet<Payment> Payments => Set<Payment>();

        // Review & Audit
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── User ──
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).IsRequired().HasMaxLength(255);
                e.Property(u => u.PasswordHash).HasMaxLength(512);
                e.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                e.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                e.Property(u => u.PhoneNumber).HasMaxLength(20);
                e.Property(u => u.AvatarUrl).HasMaxLength(1024);
                e.Property(u => u.MfaSecret).HasMaxLength(256);
                e.Property(u => u.OAuthProvider).HasMaxLength(50);
                e.Property(u => u.OAuthId).HasMaxLength(256);
                e.Property(u => u.RefreshToken).HasMaxLength(512);
            });

            // ── City ──
            modelBuilder.Entity<City>(e =>
            {
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
                e.Property(c => c.State).IsRequired().HasMaxLength(100);
                e.Property(c => c.Country).IsRequired().HasMaxLength(100);
            });

            // ── Venue ──
            modelBuilder.Entity<Venue>(e =>
            {
                e.Property(v => v.Name).IsRequired().HasMaxLength(200);
                e.Property(v => v.AddressLine1).IsRequired().HasMaxLength(255);
                e.Property(v => v.AddressLine2).HasMaxLength(255);
                e.Property(v => v.PostalCode).IsRequired().HasMaxLength(20);
                e.Property(v => v.Timezone).IsRequired().HasMaxLength(60);
                e.Property(v => v.Latitude).HasColumnType("decimal(10,8)");
                e.Property(v => v.Longitude).HasColumnType("decimal(11,8)");
                e.Property(v => v.ContactEmail).HasMaxLength(255);
                e.Property(v => v.ContactPhone).HasMaxLength(20);
                e.Property(v => v.WebsiteUrl).HasMaxLength(512);

                e.HasOne(v => v.City)
                    .WithMany(c => c.Venues)
                    .HasForeignKey(v => v.CityId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(v => v.Events)
                    .WithOne(ev => ev.Venue)
                    .HasForeignKey(ev => ev.VenueId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(v => v.OperatingHours)
                    .WithOne(oh => oh.Venue)
                    .HasForeignKey(oh => oh.VenueId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── VenueOperatingHours ──
            modelBuilder.Entity<VenueOperatingHours>(e =>
            {
                e.Property(oh => oh.OpenTime).IsRequired();
                e.Property(oh => oh.CloseTime).IsRequired();
            });

            // ── Event ──
            modelBuilder.Entity<Event>(e =>
            {
                e.Property(ev => ev.Title).IsRequired().HasMaxLength(300);
                e.Property(ev => ev.Slug).IsRequired().HasMaxLength(350);
                e.HasIndex(ev => ev.Slug).IsUnique();
                e.Property(ev => ev.Timezone).IsRequired().HasMaxLength(60);
                e.Property(ev => ev.Language).HasMaxLength(10);
                e.Property(ev => ev.BannerImageUrl).HasMaxLength(1024);
                e.Property(ev => ev.RecurrenceRule).HasMaxLength(500);

                e.HasMany(ev => ev.Seats)
                    .WithOne(s => s.Event)
                    .HasForeignKey(s => s.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(ev => ev.Bookings)
                    .WithOne(b => b.Event)
                    .HasForeignKey(b => b.EventId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Seat ──
            modelBuilder.Entity<Seat>(e =>
            {
                e.Property(s => s.SeatNumber).IsRequired().HasMaxLength(10);
                e.Property(s => s.Section).HasMaxLength(50);
                e.Property(s => s.Row).HasMaxLength(10);
                e.Property(s => s.BasePrice).HasColumnType("decimal(10,2)");
                e.Property(s => s.Currency).IsRequired().HasColumnType("char(3)");
                e.Property(s => s.RowVersion).IsRowVersion();
            });

            // ── Booking ──
            modelBuilder.Entity<Booking>(e =>
            {
                e.Property(b => b.BookingReference).IsRequired().HasMaxLength(20);
                e.HasIndex(b => b.BookingReference).IsUnique();
                e.Property(b => b.SubTotal).HasColumnType("decimal(10,2)");
                e.Property(b => b.DiscountAmount).HasColumnType("decimal(10,2)");
                e.Property(b => b.TaxAmount).HasColumnType("decimal(10,2)");
                e.Property(b => b.ConvenienceFee).HasColumnType("decimal(10,2)");
                e.Property(b => b.TotalAmount).HasColumnType("decimal(10,2)");
                e.Property(b => b.Currency).IsRequired().HasColumnType("char(3)");
                e.Property(b => b.CancellationReason).HasMaxLength(500);

                e.HasOne(b => b.User)
                    .WithMany(u => u.Bookings)
                    .HasForeignKey(b => b.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(b => b.BookingSeats)
                    .WithOne(bs => bs.Booking)
                    .HasForeignKey(bs => bs.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(b => b.Payment)
                    .WithOne(p => p.Booking)
                    .HasForeignKey<Payment>(p => p.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── BookingSeat ──
            modelBuilder.Entity<BookingSeat>(e =>
            {
                e.Property(bs => bs.PriceAtBooking).HasColumnType("decimal(10,2)");
                e.HasIndex(bs => new { bs.BookingId, bs.SeatId }).IsUnique();

                e.HasOne(bs => bs.Seat)
                    .WithMany()
                    .HasForeignKey(bs => bs.SeatId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ── Payment ──
            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(p => p.RazorpayOrderId).IsRequired().HasMaxLength(100);
                e.Property(p => p.RazorpayPaymentId).HasMaxLength(100);
                e.Property(p => p.RazorpaySignature).HasMaxLength(512);
                e.Property(p => p.Amount).HasColumnType("decimal(10,2)");
                e.Property(p => p.Currency).IsRequired().HasColumnType("char(3)");
                e.Property(p => p.GatewayFee).HasColumnType("decimal(10,2)");
                e.Property(p => p.RefundAmount).HasColumnType("decimal(10,2)");
                e.Property(p => p.RefundRazorpayId).HasMaxLength(100);
                e.Property(p => p.RefundReason).HasMaxLength(500);
                e.Property(p => p.InvoiceNumber).HasMaxLength(50);
                e.Property(p => p.FailureReason).HasMaxLength(500);
                e.HasIndex(p => p.BookingId).IsUnique();
            });

            // ── Review ──
            modelBuilder.Entity<Review>(e =>
            {
                e.Property(r => r.Title).HasMaxLength(200);

                e.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Booking)
                    .WithMany()
                    .HasForeignKey(r => r.BookingId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ── Coupon ──
            modelBuilder.Entity<Coupon>(e =>
            {
                e.Property(c => c.Code).IsRequired().HasMaxLength(50);
                e.Property(c => c.Description).HasMaxLength(300);
                e.Property(c => c.DiscountValue).HasColumnType("decimal(10,2)");
                e.Property(c => c.MaxDiscount).HasColumnType("decimal(10,2)");
                e.Property(c => c.MinPurchaseAmt).HasColumnType("decimal(10,2)");
                e.HasIndex(c => new { c.Code, c.TenantId }).IsUnique();
            });

            // ── AuditLog ──
            modelBuilder.Entity<AuditLog>(e =>
            {
                e.Property(a => a.ActionType).IsRequired().HasMaxLength(50);
                e.Property(a => a.EntityType).IsRequired().HasMaxLength(100);
                e.Property(a => a.IpAddress).HasMaxLength(45);
                e.Property(a => a.UserAgent).HasMaxLength(500);
            });
        }
    }
}
