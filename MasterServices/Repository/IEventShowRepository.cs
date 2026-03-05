using TheSeatLineApi.MasterServices.DTOs;

public interface IEventShowRepository
{
    Task<List<EventShowSelectDTO>> GetAllAsync(Guid? eventId = null, int page = 1, int pageSize = 20);
    Task<Guid> CreateAsync(EventShowInsertDTO dto);
}
