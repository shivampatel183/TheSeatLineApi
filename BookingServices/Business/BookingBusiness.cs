using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.BookingServices.DTOs;
using TheSeatLineApi.BookingServices.Repository;
using TheSeatLineApi.Common.Enums;
using TheSeatLineApi.Data;

namespace TheSeatLineApi.BookingServices.Business
{
    public class BookingBusiness : IBookingRepository
    {
        private readonly AppDbContext _context;

        public BookingBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookingResponseDto> CreateAsync(Guid userId, CreateBookingRequestDto dto)
        {
            // 1. Validate seats exist, belong to the event, and are Available
            var seats = await _context.Seats
                .Where(s => dto.SeatIds.Contains(s.Id) && s.EventId == dto.EventId)
                .ToListAsync();

            if (seats.Count != dto.SeatIds.Count)
                throw new Exception("One or more seats are invalid or do not belong to this event");

            var unavailable = seats.Where(s => s.Status != (byte)SeatStatus.Available).ToList();
            if (unavailable.Any())
                throw new Exception($"Seats are not available: {string.Join(", ", unavailable.Select(s => s.SeatNumber))}");

            // 2. Calculate totals
            var subTotal = seats.Sum(s => s.BasePrice);
            var convenienceFee = Math.Round(subTotal * 0.02m, 2); // 2% convenience fee
            var taxAmount = Math.Round(subTotal * 0.18m, 2); // 18% GST
            var totalAmount = subTotal + convenienceFee + taxAmount;

            // 3. Create booking
            var booking = new Booking
            {
                BookingReference = GenerateBookingReference(),
                UserId = userId,
                EventId = dto.EventId,
                Status = (byte)BookingStatus.Pending,
                SubTotal = subTotal,
                DiscountAmount = 0,
                TaxAmount = taxAmount,
                ConvenienceFee = convenienceFee,
                TotalAmount = totalAmount,
                HoldExpiresAt = DateTime.UtcNow.AddMinutes(10),
                SpecialRequests = dto.SpecialRequests
            };

            _context.Bookings.Add(booking);

            // 4. Create booking seats and mark seats as Reserved
            foreach (var seat in seats)
            {
                seat.Status = (byte)SeatStatus.Reserved;

                _context.BookingSeats.Add(new BookingSeat
                {
                    BookingId = booking.Id,
                    SeatId = seat.Id,
                    PriceAtBooking = seat.BasePrice
                });
            }

            await _context.SaveChangesAsync();

            return MapToResponse(booking, seats);
        }

        public async Task<List<BookingResponseDto>> GetByUserAsync(Guid userId)
        {
            var bookings = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.UserId == userId && !b.IsDeleted)
                .Include(b => b.Event)
                    .ThenInclude(e => e.Venue)
                        .ThenInclude(v => v.City)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return bookings.Select(b => MapToResponse(b, b.BookingSeats.Select(bs => bs.Seat).ToList())).ToList();
        }

        public async Task<BookingResponseDto?> GetByIdAsync(Guid bookingId, Guid userId)
        {
            var booking = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Id == bookingId && b.UserId == userId && !b.IsDeleted)
                .Include(b => b.Event)
                    .ThenInclude(e => e.Venue)
                        .ThenInclude(v => v.City)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefaultAsync();

            if (booking == null) return null;
            return MapToResponse(booking, booking.BookingSeats.Select(bs => bs.Seat).ToList());
        }

        public async Task CancelAsync(Guid bookingId, Guid userId, string? reason)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId && !b.IsDeleted)
                ?? throw new Exception("Booking not found");

            if (booking.Status == (byte)BookingStatus.Cancelled)
                throw new Exception("Booking is already cancelled");

            if (booking.Status == (byte)BookingStatus.Refunded)
                throw new Exception("Booking is already refunded");

            booking.Status = (byte)BookingStatus.Cancelled;
            booking.CancelledAt = DateTime.UtcNow;
            booking.CancellationReason = reason;
            booking.UpdatedAt = DateTime.UtcNow;

            // Release seats
            foreach (var bs in booking.BookingSeats)
            {
                bs.Seat.Status = (byte)SeatStatus.Available;
            }

            await _context.SaveChangesAsync();
        }

        public async Task TransferAsync(Guid bookingId, Guid userId, TransferBookingRequestDto dto)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == userId && !b.IsDeleted)
                ?? throw new Exception("Booking not found");

            if (booking.Status != (byte)BookingStatus.Confirmed)
                throw new Exception("Only confirmed bookings can be transferred");

            var recipient = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.RecipientEmail && !u.IsDeleted)
                ?? throw new Exception("Recipient user not found");

            booking.UserId = recipient.Id;
            booking.Status = (byte)BookingStatus.Transfered;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid bookingId, UpdateBookingStatusDto dto)
        {
            var booking = await _context.Bookings
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat)
                .FirstOrDefaultAsync(b => b.Id == bookingId && !b.IsDeleted)
                ?? throw new Exception("Booking not found");

            booking.Status = (byte)dto.BookingStatus;
            booking.UpdatedAt = DateTime.UtcNow;

            if (dto.BookingStatus == BookingStatus.Confirmed)
            {
                foreach (var bs in booking.BookingSeats)
                    bs.Seat.Status = (byte)SeatStatus.Booked;
            }
            else if (dto.BookingStatus == BookingStatus.Cancelled)
            {
                booking.CancelledAt = DateTime.UtcNow;
                booking.CancellationReason = dto.CancellationReason;
                foreach (var bs in booking.BookingSeats)
                    bs.Seat.Status = (byte)SeatStatus.Available;
            }

            await _context.SaveChangesAsync();
        }

        private static BookingResponseDto MapToResponse(Booking booking, List<Seat> seats)
        {
            return new BookingResponseDto
            {
                Id = booking.Id,
                BookingReference = booking.BookingReference,
                UserId = booking.UserId,
                EventId = booking.EventId,
                Status = ((BookingStatus)booking.Status).ToString(),
                SubTotal = booking.SubTotal,
                DiscountAmount = booking.DiscountAmount,
                TaxAmount = booking.TaxAmount,
                ConvenienceFee = booking.ConvenienceFee,
                TotalAmount = booking.TotalAmount,
                Currency = booking.Currency,
                HoldExpiresAt = booking.HoldExpiresAt,
                CancelledAt = booking.CancelledAt,
                CancellationReason = booking.CancellationReason,
                CreatedAt = booking.CreatedAt,
                Seats = seats.Select(s => new BookingSeatDto
                {
                    SeatId = s.Id,
                    SeatNumber = s.SeatNumber,
                    SeatType = ((SeatType)s.SeatType).ToString(),
                    Row = s.Row ?? "",
                    Price = s.BasePrice
                }).ToList()
            };
        }

        private static string GenerateBookingReference()
        {
            return "TSL" + DateTime.UtcNow.ToString("yyyyMMdd") + Guid.NewGuid().ToString("N")[..6].ToUpper();
        }
    }
}
