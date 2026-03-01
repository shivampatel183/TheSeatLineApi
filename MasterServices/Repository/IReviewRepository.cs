using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface IReviewRepository
    {
        Task<List<ReviewSelectDto>> GetByEventAsync(Guid eventId);
        Task<List<ReviewSelectDto>> GetByVenueAsync(Guid venueId);
        Task<Guid> CreateAsync(Guid userId, ReviewInsertDto dto);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
