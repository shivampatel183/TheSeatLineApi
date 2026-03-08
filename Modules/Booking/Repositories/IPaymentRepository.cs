using TheSeatLineApi.Modules.BookingModule.Models.DTOs;

namespace TheSeatLineApi.Modules.BookingModule.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateOrderAsync(Guid bookingId);
        Task<Payment> VerifyPaymentAsync(PaymentVerifyDto dto);
        Task<Payment> RefundAsync(Guid paymentId, string reason);
    }
}



