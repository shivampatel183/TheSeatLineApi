using TheSeatLineApi.BookingServices.Entity;

namespace TheSeatLineApi.BookingServices.Repository
{
    public interface IBookingRepository
    {
        Task<BookingEntity> CreateBookingAsync(BookingEntity booking);
        Task<BookingEntity?> GetBookingByIdAsync(Guid bookingId);
        Task<List<BookingEntity>> GetUserBookingsAsync(Guid userId);
        Task<BookingEntity> UpdateBookingAsync(BookingEntity booking);
        Task<List<BookingEntity>> GetExpiredBookingsAsync();
        Task<int> GetAvailableSeatsAsync(int showSeatCategoryId);
        Task<bool> UpdateAvailableSeatsAsync(int showSeatCategoryId, int seatsToDeduct);
        Task<bool> ReleaseSeatsAsync(int showSeatCategoryId, int seatsToRelease);
        Task<Guid?> GetUserIdByEmailAsync(string email);
    }
}
