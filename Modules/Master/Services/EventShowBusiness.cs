using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Infrastructure.Persistence;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

namespace TheSeatLineApi.Modules.MasterModule.Services
{
    public class EventShowBusiness : IEventShowRepository
    {
        private readonly AppDbContext _context;

        public EventShowBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EventShowSelectDTO>> GetAllAsync(Guid? eventId = null, int page = 1, int pageSize = 20)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = _context.EventShows
                .AsNoTracking()
                .Include(es => es.Event)
                .AsQueryable();

            if (eventId.HasValue)
                query = query.Where(es => es.EventId == eventId.Value);

            return await query
                .OrderBy(es => es.StartDateTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(es => new EventShowSelectDTO
                {
                    Id = es.Id,
                    EventId = es.EventId,
                    EventTitle = es.Event.Title,
                    StartDateTime = es.StartDateTime,
                    EndDateTime = es.EndDateTime,
                    Status = es.Status,
                    MaxCapacity = es.MaxCapacity
                })
                .ToListAsync();
        }

        public async Task<Guid> CreateAsync(EventShowInsertDTO dto)
        {
            var eventExists = await _context.Events
                .AsNoTracking()
                .AnyAsync(e => e.Id == dto.EventId && !e.IsDeleted);

            if (!eventExists)
                throw new Exception("Event not found");

            var show = new EventShow
            {
                EventId = dto.EventId,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Status = dto.Status,
                MaxCapacity = dto.MaxCapacity
            };

            _context.EventShows.Add(show);
            await _context.SaveChangesAsync();
            return show.Id;
        }
    }
}



