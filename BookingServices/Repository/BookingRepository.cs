using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.BookingServices.Entity;
using TheSeatLineApi.Common.Enums;
using TheSeatLineApi.Data;

namespace TheSeatLineApi.BookingServices.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookingEntity> CreateBookingAsync(BookingEntity booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<BookingEntity?> GetBookingByIdAsync(Guid bookingId)
        {
            return await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Show)
                    .ThenInclude(s => s.Event)
                .Include(b => b.Show)
                    .ThenInclude(s => s.Venue)
                        .ThenInclude(v => v.City)
                .Include(b => b.Show)
                    .ThenInclude(s => s.ShowSeatCategories)
                .Include(b => b.ShowSeatCategory)
                .FirstOrDefaultAsync(b => b.Id == bookingId);
        }

        public async Task<List<BookingEntity>> GetUserBookingsAsync(Guid userId)
        {
            return await _context.Bookings
                .Include(b => b.Show)
                    .ThenInclude(s => s.Event)
                .Include(b => b.Show)
                    .ThenInclude(s => s.Venue)
                        .ThenInclude(v => v.City)
                .Include(b => b.Show)
                    .ThenInclude(s => s.ShowSeatCategories)
                .Include(b => b.ShowSeatCategory)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<BookingEntity> UpdateBookingAsync(BookingEntity booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
            return booking;
        }

        public async Task<List<BookingEntity>> GetExpiredBookingsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Bookings
                .Where(b => b.BookingStatus == BookingStatus.Pending 
                    && b.ExpiryTime.HasValue 
                    && b.ExpiryTime.Value < now)
                .ToListAsync();
        }

        public async Task<int> GetAvailableSeatsAsync(int showSeatCategoryId)
        {
            var seatCategory = await _context.ShowSeatCategories
                .FirstOrDefaultAsync(sc => sc.Id == showSeatCategoryId);
            
            return seatCategory?.AvailableSeats ?? 0;
        }

        public async Task<bool> UpdateAvailableSeatsAsync(int showSeatCategoryId, int seatsToDeduct)
        {
            var seatCategory = await _context.ShowSeatCategories
                .FirstOrDefaultAsync(sc => sc.Id == showSeatCategoryId);

            if (seatCategory == null || seatCategory.AvailableSeats < seatsToDeduct)
                return false;

            seatCategory.AvailableSeats -= seatsToDeduct;
            seatCategory.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReleaseSeatsAsync(int showSeatCategoryId, int seatsToRelease)
        {
            var seatCategory = await _context.ShowSeatCategories
                .FirstOrDefaultAsync(sc => sc.Id == showSeatCategoryId);

            if (seatCategory == null)
                return false;

            seatCategory.AvailableSeats += seatsToRelease;
            seatCategory.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Guid?> GetUserIdByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            
            return user?.Id;
        }
    }
}
