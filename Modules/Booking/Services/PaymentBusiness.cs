using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Modules.BookingModule.Models.DTOs;
using TheSeatLineApi.Modules.BookingModule.Repositories;
using TheSeatLineApi.Shared.Enums;
using TheSeatLineApi.Infrastructure.Persistence;

namespace TheSeatLineApi.Modules.BookingModule.Services
{
    public class PaymentBusiness : IPaymentRepository
    {
        private readonly AppDbContext _context;

        public PaymentBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreateOrderAsync(Guid bookingId)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == bookingId && !b.IsDeleted)
                ?? throw new Exception("Booking not found");

            if (booking.Status != (byte)BookingStatus.Pending)
                throw new Exception("Payment can only be initiated for pending bookings");

            // Check if payment already exists
            var existing = await _context.Payments
                .FirstOrDefaultAsync(p => p.BookingId == bookingId);

            if (existing != null)
                return existing;

            // In production, you would call Razorpay API here to create an order
            // For now, generate a placeholder order ID
            var payment = new Payment
            {
                BookingId = bookingId,
                RazorpayOrderId = "order_" + Guid.NewGuid().ToString("N")[..16],
                Status = (byte)PaymentStatus.Initiated,
                Amount = booking.TotalAmount,
                Method = 0
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> VerifyPaymentAsync(PaymentVerifyDto dto)
        {
            var payment = await _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Tickets)
                        .ThenInclude(t => t.Seat)
                .FirstOrDefaultAsync(p => p.BookingId == dto.BookingId
                    && p.RazorpayOrderId == dto.RazorpayOrderId)
                ?? throw new Exception("Payment not found");

            // In production, verify signature with Razorpay SDK
            // For now, mark as success
            payment.RazorpayPaymentId = dto.RazorpayPaymentId;
            payment.RazorpaySignature = dto.RazorpaySignature;
            payment.Status = (byte)PaymentStatus.Success;
            payment.UpdatedAt = DateTime.UtcNow;

            // Confirm the booking and mark seats as booked
            payment.Booking.Status = (byte)BookingStatus.Confirmed;
            payment.Booking.UpdatedAt = DateTime.UtcNow;

            foreach (var ticket in payment.Booking.Tickets)
            {
                if(ticket.Seat != null) ticket.Seat.Status = (byte)SeatStatus.Booked;
                ticket.Status = (byte)TicketStatus.Valid;
            }

            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<Payment> RefundAsync(Guid paymentId, string reason)
        {
            var payment = await _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Tickets)
                        .ThenInclude(t => t.Seat)
                .FirstOrDefaultAsync(p => p.Id == paymentId)
                ?? throw new Exception("Payment not found");

            if (payment.Status != (byte)PaymentStatus.Success)
                throw new Exception("Only successful payments can be refunded");

            // In production, call Razorpay Refund API
            payment.Status = (byte)PaymentStatus.Refunded;
            payment.RefundAmount = payment.Amount;
            payment.RefundReason = reason;
            payment.RefundedAt = DateTime.UtcNow;
            payment.RefundRazorpayId = "rfnd_" + Guid.NewGuid().ToString("N")[..16];
            payment.UpdatedAt = DateTime.UtcNow;

            // Mark booking as refunded and release seats
            payment.Booking.Status = (byte)BookingStatus.Refunded;
            payment.Booking.UpdatedAt = DateTime.UtcNow;

            foreach (var ticket in payment.Booking.Tickets)
            {
                if(ticket.Seat != null) ticket.Seat.Status = (byte)SeatStatus.Available;
                ticket.Status = (byte)TicketStatus.Cancelled;
            }

            await _context.SaveChangesAsync();
            return payment;
        }
    }
}



