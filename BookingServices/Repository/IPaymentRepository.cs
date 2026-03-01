using TheSeatLineApi.BookingServices.DTOs;

namespace TheSeatLineApi.BookingServices.Repository
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateOrderAsync(Guid bookingId);
        Task<Payment> VerifyPaymentAsync(PaymentVerifyDto dto);
        Task<Payment> RefundAsync(Guid paymentId, string reason);
    }
}
