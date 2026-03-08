using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

namespace TheSeatLineApi.Modules.MasterModule.Repositories
{
    public interface IVenueRepository
    {
        Task<List<VenueListDto>> GetAllAsync();
        Task<VenueDetailDto?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(VenueCreateDto dto);
        Task UpdateAsync(Guid id, VenueUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}



