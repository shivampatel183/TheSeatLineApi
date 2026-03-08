using TheSeatLineApi.Modules.BookingModule.Models.DTOs;

namespace TheSeatLineApi.Modules.BookingModule.Repositories
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



