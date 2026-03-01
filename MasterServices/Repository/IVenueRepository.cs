using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface IVenueRepository
    {
        Task<List<VenueSummaryDto>> GetAllAsync();
        Task<Venue?> GetByIdAsync(Guid id);
        Task<List<VenueSummaryDto>> GetByCityAsync(Guid cityId);
        Task<Guid> CreateAsync(CreateVenueRequestDto dto);
        Task UpdateAsync(Guid id, CreateVenueRequestDto dto);
        Task DeleteAsync(Guid id);
    }
}
