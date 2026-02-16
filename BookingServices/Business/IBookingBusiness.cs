using TheSeatLineApi.BookingServices.DTOs;
using TheSeatLineApi.Common;

namespace TheSeatLineApi.BookingServices.Business
{
    public interface IBookingBusiness
    {
        Task<Response<BookingResponseDto>> CreateBookingAsync(Guid userId, CreateBookingRequestDto request);
        Task<Response<BookingResponseDto>> GetBookingDetailsAsync(Guid bookingId, Guid userId);
        Task<Response<List<BookingResponseDto>>> GetUserBookingsAsync(Guid userId);
        Task<Response<BookingResponseDto>> ConfirmBookingAsync(Guid bookingId, Guid userId);
        Task<Response<BookingResponseDto>> CancelBookingAsync(Guid bookingId, Guid userId, string? cancellationReason);
        Task<Response<BookingResponseDto>> TransferBookingAsync(Guid bookingId, Guid currentUserId, TransferBookingRequestDto request);
        Task ExpireBookingsAsync();
    }
}
