using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

public interface IEventShowRepository
{
    Task<List<EventShowSelectDTO>> GetAllAsync(Guid? eventId = null, int page = 1, int pageSize = 20);
    Task<List<EventShowSelectDTO>> GetByEventIdAsync(Guid eventId);
    Task<Guid> CreateAsync(EventShowInsertDTO dto);
    Task UpdateAsync(Guid id, EventShowUpdateDTO dto);
    Task DeleteAsync(Guid id);
}



