using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface IShowRepository
    {
        Task<List<ShowSelectDTO>> SelectShow();

        Task<List<ShowSelectDTO>> SelectShowByEvent(int eventId);

        Task<List<ShowSelectDTO>> SelectShowByVenue(int venueId);

        Task<ShowSelectDTO?> SelectShowById(int id);

        Task<int> InsertShow(ShowInsertDTO dto);

        Task<int> UpdateShow(ShowInsertDTO dto);

        Task<int> DeleteShow(int showId);
    }
}
