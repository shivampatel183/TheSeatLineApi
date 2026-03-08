using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

namespace TheSeatLineApi.Modules.MasterModule.Repositories
{
    public interface IReviewRepository
    {
        Task<List<ReviewSelectDto>> GetByEventAsync(Guid eventId);
        Task<List<ReviewSelectDto>> GetByVenueAsync(Guid venueId);
        Task<Guid> CreateAsync(Guid userId, ReviewInsertDto dto);
        Task DeleteAsync(Guid id, Guid userId);
    }
}



