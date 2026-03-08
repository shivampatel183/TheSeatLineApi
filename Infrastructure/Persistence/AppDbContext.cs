using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data.Entities;
using TheSeatLineApi.Modules.AuthModule.Models.Entities;
using TheSeatLineApi.Modules.BookingModule.Models.Entities;
using TheSeatLineApi.Modules.MasterModule.Models.Entities;

namespace TheSeatLineApi.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ===== Core =====
        public DbSet<User> Users => Set<User>();
        public DbSet<Organizer> Organizers => Set<Organizer>();

        // ===== Master =====
        public DbSet<City> Cities => Set<City>();
        public DbSet<Venue> Venues => Set<Venue>();
        public DbSet<VenueOperatingHours> VenueOperatingHours => Set<VenueOperatingHours>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<EventShow> EventShows => Set<EventShow>();
        public DbSet<EventCategory> EventCategories => Set<EventCategory>();
        public DbSet<EventImage> EventImages => Set<EventImage>();
        public DbSet<EventTag> EventTags => Set<EventTag>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<SeatReservation> SeatReservations => Set<SeatReservation>();
        public DbSet<SeatCoordinate> SeatCoordinates => Set<SeatCoordinate>();
        public DbSet<TicketCategory> TicketCategories => Set<TicketCategory>();

        // ===== Booking & Ticketing =====
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingItem> BookingItems => Set<BookingItem>();
        public DbSet<BookingAddOn> BookingAddOns => Set<BookingAddOn>();
        public DbSet<AddOn> AddOns => Set<AddOn>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<TicketScan> TicketScans => Set<TicketScan>();
        public DbSet<TicketTransfer> TicketTransfers => Set<TicketTransfer>();
        public DbSet<WaitingList> WaitingLists => Set<WaitingList>();

        // ===== Coupons & Discounts =====
        public DbSet<Coupon> Coupons => Set<Coupon>();
        public DbSet<CouponEvent> CouponEvents => Set<CouponEvent>();

        // ===== Payment =====
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<PaymentEvent> PaymentEvents => Set<PaymentEvent>();
        public DbSet<PaymentRefund> PaymentRefunds => Set<PaymentRefund>();

        // ===== Price History =====
        public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

        // ===== Reviews & Audit =====
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        // ===== Notifications =====
        public DbSet<NotificationTemplate> NotificationTemplates => Set<NotificationTemplate>();
        public DbSet<UserNotificationPref> UserNotificationPrefs => Set<UserNotificationPref>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ---------- User ----------
            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Email).IsRequired().HasMaxLength(255);
                e.Property(u => u.PasswordHash).HasMaxLength(512);
                e.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
                e.Property(u => u.LastName).HasMaxLength(100);
                e.Property(u => u.PhoneNumber).HasMaxLength(20);
                e.Property(u => u.AvatarUrl).HasMaxLength(1024);
                e.Property(u => u.MfaSecret).HasMaxLength(256);
                e.Property(u => u.OAuthProvider).HasMaxLength(50);
                e.Property(u => u.OAuthId).HasMaxLength(256);
                e.Property(u => u.RefreshToken).HasMaxLength(512);

                //// Events organized by this user (optional)
                //e.HasMany(u => u.OrganizedEvents)
                //    .WithOne(ev => ev.Organizer)
                //    .HasForeignKey(ev => ev.OrganizerId)
                //    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- City ----------
            modelBuilder.Entity<City>(e =>
            {
                e.Property(c => c.Name).IsRequired().HasMaxLength(100);
                e.Property(c => c.State).IsRequired().HasMaxLength(100);
                e.Property(c => c.Country).IsRequired().HasMaxLength(100);
            });

            // ---------- Venue ----------
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

                e.Property(v => v.Amenities).HasColumnType("nvarchar(max)");
                e.Property(v => v.AccessibilityFeatures).HasColumnType("nvarchar(max)");
                e.Property(v => v.MediaGallery).HasColumnType("nvarchar(max)");

                e.ToTable(tb => tb.HasCheckConstraint("CK_Venues_Amenities_JSON", "Amenities IS NULL OR ISJSON(Amenities) = 1"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Venues_Access_JSON", "AccessibilityFeatures IS NULL OR ISJSON(AccessibilityFeatures) = 1"));
                e.ToTable(tb => tb.HasCheckConstraint("CK_Venues_Media_JSON", "MediaGallery IS NULL OR ISJSON(MediaGallery) = 1"));

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

            // ---------- VenueOperatingHours ----------
            modelBuilder.Entity<VenueOperatingHours>(e =>
            {
                e.Property(oh => oh.OpenTime).IsRequired();
                e.Property(oh => oh.CloseTime).IsRequired();
            });

            // ---------- Event ----------
            modelBuilder.Entity<Event>(e =>
            {
                e.Property(ev => ev.Title).IsRequired().HasMaxLength(300);
                e.Property(ev => ev.Slug).IsRequired().HasMaxLength(350);
                e.HasIndex(ev => ev.Slug).IsUnique();
                e.HasIndex(ev => new { ev.VenueId, ev.StartDateTime });
                e.Property(ev => ev.Timezone).IsRequired().HasMaxLength(60);
                e.Property(ev => ev.Language).HasMaxLength(10);
                e.Property(ev => ev.BannerImageUrl).HasMaxLength(1024);
                e.Property(ev => ev.RecurrenceRule).HasMaxLength(500);

                // Performers stored as JSON
                e.Property(ev => ev.Performers).HasColumnType("nvarchar(max)");
                e.ToTable(tb => tb.HasCheckConstraint("CK_Events_Performers_JSON", "Performers IS NULL OR ISJSON(Performers) = 1"));

                // Relationships
                e.HasOne(ev => ev.Organizer)
                    .WithMany(u => u.OrganizedEvents)
                    .HasForeignKey(ev => ev.OrganizerId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ev => ev.Venue)
                    .WithMany(v => v.Events)
                    .HasForeignKey(ev => ev.VenueId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ev => ev.Category)
                    .WithMany(c => c.Events)
                    .HasForeignKey(ev => ev.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);

                e.HasMany(ev => ev.Shows)
                    .WithOne(s => s.Event)
                    .HasForeignKey(s => s.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(ev => ev.Images)
                    .WithOne(i => i.Event)
                    .HasForeignKey(i => i.EventId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(ev => ev.EventTags)
                    .WithOne(et => et.Event)
                    .HasForeignKey(et => et.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- EventCategory ----------
            modelBuilder.Entity<EventCategory>(e =>
            {
                e.Property(ec => ec.Name).IsRequired().HasMaxLength(100);
                e.Property(ec => ec.Slug).IsRequired().HasMaxLength(120);
                e.HasIndex(ec => ec.Slug).IsUnique();
            });

            // ---------- EventImage ----------
            modelBuilder.Entity<EventImage>(e =>
            {
                e.Property(ei => ei.ImageUrl).IsRequired().HasMaxLength(1024);
                e.HasOne(ei => ei.Event)
                    .WithMany(ev => ev.Images)
                    .HasForeignKey(ei => ei.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- EventTag ----------
            modelBuilder.Entity<EventTag>(e =>
            {
                e.Property(et => et.Tag).IsRequired().HasMaxLength(50);
                e.HasOne(et => et.Event)
                    .WithMany(ev => ev.EventTags)
                    .HasForeignKey(et => et.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- EventShow ----------
            modelBuilder.Entity<EventShow>(e =>
            {
                e.HasMany(es => es.TicketCategories)
                    .WithOne(tc => tc.EventShow)
                    .HasForeignKey(tc => tc.EventShowId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(es => es.Seats)
                    .WithOne(s => s.EventShow)
                    .HasForeignKey(s => s.EventShowId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(es => es.Bookings)
                    .WithOne(b => b.EventShow)
                    .HasForeignKey(b => b.EventShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(es => es.Tickets)
                    .WithOne(t => t.EventShow)
                    .HasForeignKey(t => t.EventShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(es => es.SeatReservations)
                    .WithOne(sr => sr.EventShow)
                    .HasForeignKey(sr => sr.EventShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(es => es.WaitingLists)
                    .WithOne(wl => wl.EventShow)
                    .HasForeignKey(wl => wl.EventShowId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- TicketCategory ----------
            modelBuilder.Entity<TicketCategory>(e =>
            {
                e.Property(tc => tc.Name).IsRequired().HasMaxLength(100);
                e.Property(tc => tc.Price).HasColumnType("decimal(10,2)");
                e.Property(tc => tc.RowVersion).IsRowVersion(); // concurrency token
            });

            // ---------- Seat ----------
            modelBuilder.Entity<Seat>(e =>
            {
                e.Property(s => s.SeatNumber).IsRequired().HasMaxLength(10);
                e.Property(s => s.Section).HasMaxLength(50);
                e.Property(s => s.Row).HasMaxLength(10);
                e.Property(s => s.BasePrice).HasColumnType("decimal(10,2)");
                e.Property(s => s.Currency).IsRequired().HasColumnType("char(3)");
                e.Property(s => s.RowVersion).IsRowVersion();

                e.HasOne(s => s.EventShow)
                    .WithMany(es => es.Seats)
                    .HasForeignKey(s => s.EventShowId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(s => s.Coordinates)
                    .WithOne(sc => sc.Seat)
                    .HasForeignKey<SeatCoordinate>(sc => sc.SeatId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- SeatCoordinate ----------
            modelBuilder.Entity<SeatCoordinate>(e =>
            {
                e.Property(sc => sc.Shape).HasColumnType("nvarchar(max)");
            });

            // ---------- SeatReservation ----------
            modelBuilder.Entity<SeatReservation>(e =>
            {
                e.HasIndex(sr => new { sr.SeatId, sr.EventShowId });
                e.HasIndex(sr => sr.ExpiresAt); // for cleanup job

                e.HasOne(sr => sr.Seat)
                    .WithMany(s => s.SeatReservations)
                    .HasForeignKey(sr => sr.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(sr => sr.EventShow)
                    .WithMany(es => es.SeatReservations)
                    .HasForeignKey(sr => sr.EventShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(sr => sr.User)
                    .WithMany(u => u.SeatReservations)
                    .HasForeignKey(sr => sr.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Booking ----------
            modelBuilder.Entity<Booking>(e =>
            {
                e.Property(b => b.BookingReference).IsRequired().HasMaxLength(20);
                e.HasIndex(b => b.BookingReference).IsUnique();
                e.HasIndex(b => new { b.UserId, b.CreatedAt });

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

                e.HasOne(b => b.EventShow)
                    .WithMany(es => es.Bookings)
                    .HasForeignKey(b => b.EventShowId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(b => b.Items)
                    .WithOne(bi => bi.Booking)
                    .HasForeignKey(bi => bi.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(b => b.AddOns)
                    .WithOne(ba => ba.Booking)
                    .HasForeignKey(ba => ba.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(b => b.Tickets)
                    .WithOne(t => t.Booking)
                    .HasForeignKey(t => t.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(b => b.Payment)
                    .WithOne(p => p.Booking)
                    .HasForeignKey<Payment>(p => p.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- BookingItem ----------
            modelBuilder.Entity<BookingItem>(e =>
            {
                e.Property(bi => bi.Price).HasColumnType("decimal(10,2)");

                e.HasOne(bi => bi.Booking)
                    .WithMany(b => b.Items)
                    .HasForeignKey(bi => bi.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(bi => bi.Seat)
                    .WithMany()
                    .HasForeignKey(bi => bi.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(bi => bi.TicketCategory)
                    .WithMany()
                    .HasForeignKey(bi => bi.TicketCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- AddOn ----------
            modelBuilder.Entity<AddOn>(e =>
            {
                e.Property(a => a.Name).IsRequired().HasMaxLength(200);
                e.Property(a => a.Price).HasColumnType("decimal(10,2)");
                e.Property(a => a.Currency).IsRequired().HasColumnType("char(3)");
            });

            // ---------- BookingAddOn ----------
            modelBuilder.Entity<BookingAddOn>(e =>
            {
                e.Property(ba => ba.Price).HasColumnType("decimal(10,2)");

                e.HasOne(ba => ba.Booking)
                    .WithMany(b => b.AddOns)
                    .HasForeignKey(ba => ba.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ba => ba.AddOn)
                    .WithMany(a => a.BookingAddOns)
                    .HasForeignKey(ba => ba.AddOnId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- Ticket ----------
            modelBuilder.Entity<Ticket>(e =>
            {
                e.Property(t => t.TicketNumber).IsRequired().HasMaxLength(50);
                e.HasIndex(t => t.TicketNumber).IsUnique();
                e.Property(t => t.QRCode).IsRequired().HasMaxLength(500);

                e.ToTable(tb => tb.HasCheckConstraint("CK_Tickets_SeatOrCategory",
                    "(SeatId IS NOT NULL AND TicketCategoryId IS NULL) OR (SeatId IS NULL AND TicketCategoryId IS NOT NULL)"));

                e.HasOne(t => t.OwnerUser)
                    .WithMany()
                    .HasForeignKey(t => t.OwnerUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.TransferredToUser)
                    .WithMany()
                    .HasForeignKey(t => t.TransferredToUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.Seat)
                    .WithMany()
                    .HasForeignKey(t => t.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.TicketCategory)
                    .WithMany()
                    .HasForeignKey(t => t.TicketCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasMany(t => t.Scans)
                    .WithOne(ts => ts.Ticket)
                    .HasForeignKey(ts => ts.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(t => t.Transfers)
                    .WithOne(tt => tt.Ticket)
                    .HasForeignKey(tt => tt.TicketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- TicketScan ----------
            modelBuilder.Entity<TicketScan>(e =>
            {
                e.Property(ts => ts.ScannedAt).IsRequired();
            });

            // ---------- TicketTransfer ----------
            modelBuilder.Entity<TicketTransfer>(e =>
            {
                e.HasOne(tt => tt.FromUser)
                    .WithMany()
                    .HasForeignKey(tt => tt.FromUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(tt => tt.ToUser)
                    .WithMany()
                    .HasForeignKey(tt => tt.ToUserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- WaitingList ----------
            modelBuilder.Entity<WaitingList>(e =>
            {
                e.HasOne(wl => wl.User)
                    .WithMany()
                    .HasForeignKey(wl => wl.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(wl => wl.EventShow)
                    .WithMany(es => es.WaitingLists)
                    .HasForeignKey(wl => wl.EventShowId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- Coupon ----------
            modelBuilder.Entity<Coupon>(e =>
            {
                e.Property(c => c.Code).IsRequired().HasMaxLength(50);
                e.Property(c => c.Description).HasMaxLength(300);
                e.Property(c => c.DiscountValue).HasColumnType("decimal(10,2)");
                e.Property(c => c.MaxDiscount).HasColumnType("decimal(10,2)");
                e.Property(c => c.MinPurchaseAmt).HasColumnType("decimal(10,2)");

                // Global unique code (or combine with OrganizerId if needed)
                e.HasIndex(c => c.Code).IsUnique();

                // Optional: tie coupon to an organizer (if coupons are per partner)
                // e.HasOne(c => c.Organizer).WithMany().HasForeignKey(c => c.OrganizerId);
            });

            // ---------- CouponEvent (many-to-many) ----------
            modelBuilder.Entity<CouponEvent>(e =>
            {
                e.HasKey(ce => new { ce.CouponId, ce.EventId });

                e.HasOne(ce => ce.Coupon)
                    .WithMany()
                    .HasForeignKey(ce => ce.CouponId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(ce => ce.Event)
                    .WithMany()
                    .HasForeignKey(ce => ce.EventId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- Payment ----------
            modelBuilder.Entity<Payment>(e =>
            {
                e.Property(p => p.RazorpayOrderId).IsRequired().HasMaxLength(100);
                e.Property(p => p.RazorpayPaymentId).HasMaxLength(100);
                e.Property(p => p.RazorpaySignature).HasMaxLength(512);
                e.Property(p => p.Amount).HasColumnType("decimal(10,2)");
                e.Property(p => p.Currency).IsRequired().HasColumnType("char(3)");
                e.Property(p => p.GatewayFee).HasColumnType("decimal(10,2)");
                e.Property(p => p.FailureReason).HasMaxLength(500);
                e.HasIndex(p => p.BookingId).IsUnique();

                e.HasMany(p => p.Events)
                    .WithOne(pe => pe.Payment)
                    .HasForeignKey(pe => pe.PaymentId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(p => p.Refunds)
                    .WithOne(pr => pr.Payment)
                    .HasForeignKey(pr => pr.PaymentId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- PaymentEvent ----------
            modelBuilder.Entity<PaymentEvent>(e =>
            {
                e.Property(pe => pe.EventType).IsRequired().HasMaxLength(50);
                e.Property(pe => pe.GatewayPayload).HasColumnType("nvarchar(max)");
            });

            // ---------- PaymentRefund ----------
            modelBuilder.Entity<PaymentRefund>(e =>
            {
                e.Property(pr => pr.Amount).HasColumnType("decimal(10,2)");
                e.Property(pr => pr.RefundGatewayId).HasMaxLength(100);
                e.Property(pr => pr.Reason).HasMaxLength(500);
            });

            // ---------- PriceHistory ----------
            modelBuilder.Entity<PriceHistory>(e =>
            {
                e.Property(ph => ph.OldPrice).HasColumnType("decimal(10,2)");
                e.Property(ph => ph.NewPrice).HasColumnType("decimal(10,2)");

                e.HasOne(ph => ph.TicketCategory)
                    .WithMany(tc => tc.PriceHistories)
                    .HasForeignKey(ph => ph.TicketCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- Review ----------
            modelBuilder.Entity<Review>(e =>
            {
                e.Property(r => r.Title).HasMaxLength(200);
                e.ToTable(tb => tb.HasCheckConstraint("CK_Reviews_Rating", "Rating >= 1 AND Rating <= 5"));

                e.HasOne(r => r.User)
                    .WithMany()
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(r => r.Booking)
                    .WithMany()
                    .HasForeignKey(r => r.BookingId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ---------- NotificationTemplate ----------
            modelBuilder.Entity<NotificationTemplate>(e =>
            {
                e.Property(nt => nt.EventType).IsRequired().HasMaxLength(50);
                e.Property(nt => nt.Subject).HasMaxLength(255);
                e.Property(nt => nt.Body).IsRequired();
            });

            // ---------- UserNotificationPref ----------
            modelBuilder.Entity<UserNotificationPref>(e =>
            {
                e.HasKey(unp => new { unp.UserId, unp.NotificationType });

                e.HasOne(unp => unp.User)
                    .WithMany(u => u.NotificationPrefs)
                    .HasForeignKey(unp => unp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ---------- AuditLog ----------
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


