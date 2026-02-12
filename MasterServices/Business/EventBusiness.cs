using static TheSeatLineApi.MasterServices.DTOs.EventDTOs;
using TheSeatLineApi.Data;
using TheSeatLineApi.MasterServices.Entity;
using TheSeatLineApi.MasterServices.Repository;
using Microsoft.EntityFrameworkCore;

namespace TheSeatLineApi.MasterServices.Business
{
    public class EventBusiness: IEventRepository
    {
        private readonly AppDbContext dbcontext;

        public EventBusiness(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<List<EventSelectDTO>> SelectEvent()
        {
            return await dbcontext.Events
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => new EventSelectDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language,
                    DurationMinutes = x.DurationMinutes,
                    PosterUrl = x.PosterUrl
                })
                .ToListAsync();
        }

        public async Task<EventSelectDTO?> SelectEventById(int id)
        {
            return await dbcontext.Events
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new EventSelectDTO
                {
                    Id = x.Id,
                    Title = x.Title,
                    Language = x.Language,
                    DurationMinutes = x.DurationMinutes,
                    PosterUrl = x.PosterUrl
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> InsertEvent(EventInsertDTO dto)
        {
            dbcontext.Events.Add(new EventEntity
            {
                Title = dto.Title,
                Description = dto.Description,
                Language = dto.Language,
                DurationMinutes = dto.DurationMinutes,
                PosterUrl = dto.PosterUrl,
                IsActive = true
            });

            return await dbcontext.SaveChangesAsync();
        }

        public async Task<int> UpdateEvent(EventInsertDTO dto)
        {
            return await dbcontext.Events
                .Where(x => x.Id == dto.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Title, dto.Title)
                    .SetProperty(p => p.Description, dto.Description)
                    .SetProperty(p => p.Language, dto.Language)
                    .SetProperty(p => p.DurationMinutes, dto.DurationMinutes)
                    .SetProperty(p => p.PosterUrl, dto.PosterUrl)
                    .SetProperty(p => p.IsActive, dto.IsActive)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow)
                );
        }

        public async Task<int> DeleteEvent(int eventId)
        {
            return await dbcontext.Events
                .Where(x => x.Id == eventId)
                .ExecuteDeleteAsync();
        }
    }
}
