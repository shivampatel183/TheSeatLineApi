using TheSeatLineApi.MasterServices.DTOs;
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

            // Location-based queries
            Task<List<EventSelectDTO>> GetEventsByCityAsync(int cityId, DateTime? fromDate = null, DateTime? toDate = null);
            
            Task<List<EventSelectDTO>> GetNearbyEventsAsync(EventLocationQueryDto query);
        
    }
}
