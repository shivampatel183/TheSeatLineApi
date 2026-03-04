using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Common.Helpers;
using TheSeatLineApi.Data;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Business
{
    public class EventBusiness : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<EventSelectDTO>> GetAllAsync(EventLocationQueryDto? query = null)
        {
            var q = _context.Events
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .Include(e => e.Venue)
                    .ThenInclude(v => v.City)
                .AsQueryable();

            // Filter by city name
            if (query?.CityName != null)
            {
                var cityId = (Guid)_context.Cities
                    .Where(c => c.Name == query.CityName)
                    .Select(c => c.Id)
                    .FirstOrDefault();

                q = q.Where(e => e.Venue.CityId == cityId);
            }

            //// Filter by city
            //if (query?.CityId != null)
            //{
            //    var cityId = (Guid)_context.Cities
            //        .Where(c => c.Id == Guid.Parse(query.CityId.ToString()!))
            //        .Select(c => c.Id)
            //        .FirstOrDefault();

            //    q = q.Where(e => e.Venue.CityId == cityId);
            //}

            // Filter by date range
            if (query?.FromDate != null)
                q = q.Where(e => e.StartDateTime >= query.FromDate);

            if (query?.ToDate != null)
                q = q.Where(e => e.EndDateTime <= query.ToDate);

            // Pagination
            int page = query?.PageNumber ?? 1;
            int size = query?.PageSize ?? 20;

            var events = await q
                .OrderBy(e => e.StartDateTime)
                .Skip((page - 1) * size)
                .Take(size)
                .Select(e => new EventSelectDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventType = e.EventType,
                    Language = e.Language,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    BannerImageUrl = e.BannerImageUrl,
                    Status = e.Status,
                    VenueId = e.VenueId,
                    VenueName = e.Venue.Name,
                    City = e.Venue.City.Name,
                    State = e.Venue.City.State
                })
                .ToListAsync();

            // Apply GPS-based filtering in memory if lat/lng provided
            if (query?.Latitude != null && query?.Longitude != null)
            {
                var venuesWithCoords = await _context.Venues
                    .AsNoTracking()
                    .Where(v => v.Latitude != null && v.Longitude != null && !v.IsDeleted)
                    .Select(v => new { v.Id, v.Latitude, v.Longitude })
                    .ToListAsync();

                var nearbyVenueIds = venuesWithCoords
                    .Where(v => LocationHelper.IsWithinRadius(
                        query.Latitude.Value, query.Longitude.Value,
                        v.Latitude!.Value, v.Longitude!.Value,
                        query.RadiusKm))
                    .Select(v => v.Id)
                    .ToHashSet();

                events = events.Where(e => nearbyVenueIds.Contains(e.VenueId)).ToList();
            }

            return events;
        }

        public async Task<EventDetailDTO?> GetByIdAsync(Guid id)
        {
            return await _context.Events
                .AsNoTracking()
                .Where(e => e.Id == id && !e.IsDeleted)
                .Include(e => e.Venue)
                    .ThenInclude(v => v.City)
                .Select(e => new EventDetailDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventType = e.EventType,
                    Language = e.Language,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    BannerImageUrl = e.BannerImageUrl,
                    Status = e.Status,
                    VenueId = e.VenueId,
                    VenueName = e.Venue.Name,
                    City = e.Venue.City.Name,
                    State = e.Venue.City.State,
                    Tags = e.Tags,
                    Performers = e.Performers,
                    AgeRestriction = e.AgeRestriction,
                    Timezone = e.Timezone,
                    MaxCapacity = e.MaxCapacity
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateAsync(EventInsertDTO dto)
        {
            var slug = dto.Title.ToLower().Replace(" ", "-") + "-" + Guid.NewGuid().ToString("N")[..8];

            var ev = new Event
            {
                VenueId = dto.VenueId,
                Title = dto.Title,
                Slug = slug,
                Description = dto.Description,
                EventType = dto.EventType,
                Tags = dto.Tags,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Timezone = dto.Timezone,
                Language = dto.Language,
                MaxCapacity = dto.MaxCapacity,
                BannerImageUrl = dto.BannerImageUrl,
                Performers = dto.Performers
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();
            return ev.Id;
        }

        public async Task UpdateAsync(Guid id, EventInsertDTO dto)
        {
            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted)
                ?? throw new Exception("Event not found");

            ev.Title = dto.Title;
            ev.VenueId = dto.VenueId;
            ev.Description = dto.Description;
            ev.EventType = dto.EventType;
            ev.Tags = dto.Tags;
            ev.StartDateTime = dto.StartDateTime;
            ev.EndDateTime = dto.EndDateTime;
            ev.Timezone = dto.Timezone;
            ev.Language = dto.Language;
            ev.MaxCapacity = dto.MaxCapacity;
            ev.BannerImageUrl = dto.BannerImageUrl;
            ev.Performers = dto.Performers;
            ev.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted)
                ?? throw new Exception("Event not found");

            ev.IsDeleted = true;
            ev.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
