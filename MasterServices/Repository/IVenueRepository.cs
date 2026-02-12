using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface IVenueRepository
    {
        Task<int> InsertVenue(VenueInsertDTO venue);

        Task<int> UpdateVenue(VenueInsertDTO venue);

        Task<int> DeleteVenue(int venueId);

        Task<List<VenueSelectDTO>> SelectVenue();

        Task<List<VenueSelectDTO>> SelectVenueByCity(int cityId);

        Task<VenueSelectDTO?> SelectVenueById(int id);
    }
}
