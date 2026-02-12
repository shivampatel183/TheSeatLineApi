using static TheSeatLineApi.MasterServices.DTOs.EventDTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface IEventRepository
    {
            Task<List<EventSelectDTO>> SelectEvent();

            Task<EventSelectDTO?> SelectEventById(int id);

            Task<int> InsertEvent(EventInsertDTO dto);

            Task<int> UpdateEvent(EventInsertDTO dto);

            Task<int> DeleteEvent(int eventId);
        
    }
}
