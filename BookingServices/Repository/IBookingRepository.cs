using TheSeatLineApi.BookingServices.DTOs;

namespace TheSeatLineApi.BookingServices.Repository
{
    public interface IBookingRepository
    {
        Task<BookingResponseDto> CreateAsync(Guid userId, CreateBookingRequestDto dto);
        Task<List<BookingResponseDto>> GetByUserAsync(Guid userId);
        Task<BookingResponseDto?> GetByIdAsync(Guid bookingId, Guid userId);
        Task CancelAsync(Guid bookingId, Guid userId, string? reason);
        Task TransferAsync(Guid bookingId, Guid userId, TransferBookingRequestDto dto);
        Task UpdateStatusAsync(Guid bookingId, UpdateBookingStatusDto dto);
    }
}
