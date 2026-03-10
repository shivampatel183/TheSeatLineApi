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

        public async Task<List<EventShowSelectDTO>> GetByEventIdAsync(Guid eventId)
        {
            var eventExists = await _context.Events
                .AsNoTracking()
                .AnyAsync(e => e.Id == eventId && !e.IsDeleted);

            if (!eventExists)
                throw new Exception("Event not found");

            return await _context.EventShows
                .AsNoTracking()
                .Include(es => es.Event)
                .Where(es => es.EventId == eventId)
                .OrderBy(es => es.StartDateTime)
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

        public async Task UpdateAsync(Guid id, EventShowUpdateDTO dto)
        {
            var show = await _context.EventShows
                .FirstOrDefaultAsync(es => es.Id == id)
                ?? throw new Exception("Event show not found");

            show.StartDateTime = dto.StartDateTime;
            show.EndDateTime = dto.EndDateTime;
            show.Status = dto.Status;
            show.MaxCapacity = dto.MaxCapacity;
            show.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var show = await _context.EventShows
                .FirstOrDefaultAsync(es => es.Id == id)
                ?? throw new Exception("Event show not found");

            _context.EventShows.Remove(show);
            await _context.SaveChangesAsync();
        }
    }
}



