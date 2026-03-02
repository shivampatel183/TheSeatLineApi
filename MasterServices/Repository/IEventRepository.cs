using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface IEventRepository
    {
        Task<List<EventSelectDTO>> GetAllAsync(EventLocationQueryDto? query = null);
        Task<EventDetailDTO?> GetByIdAsync(Guid id);
        Task<Guid> CreateAsync(EventInsertDTO dto);
        Task UpdateAsync(Guid id, EventInsertDTO dto);
        Task DeleteAsync(Guid id);
    }
}
