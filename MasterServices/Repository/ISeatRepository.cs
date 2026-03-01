using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface ISeatRepository
    {
        Task<List<SeatSelectDTO>> GetByEventAsync(Guid eventId);
        Task<Guid> CreateAsync(SeatInsertDTO dto);
        Task<List<Guid>> BulkCreateAsync(SeatBulkInsertDTO dto);
        Task UpdateAsync(Guid id, SeatInsertDTO dto);
        Task DeleteAsync(Guid id);
    }
}
