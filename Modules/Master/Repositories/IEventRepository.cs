using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

public interface IEventRepository
{
    Task<List<EventSelectDTO>> GetAllAsync(EventLocationQueryDto? query = null);
    Task<EventDetailDTO?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(EventInsertDTO dto, Guid currentUserId);
    Task UpdateAsync(Guid id, EventInsertDTO dto, Guid currentUserId);
    Task DeleteAsync(Guid id, Guid currentUserId);
}


