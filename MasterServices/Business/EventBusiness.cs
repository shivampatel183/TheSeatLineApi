using static TheSeatLineApi.MasterServices.DTOs.EventDTOs;
using TheSeatLineApi.Data;
using TheSeatLineApi.MasterServices.Entity;
using TheSeatLineApi.MasterServices.Repository;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.Common.Helpers;
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
                    Description = x.Description,
                    Language = x.Language,
                    DurationMinutes = x.DurationMinutes,
                    PosterUrl = x.PosterUrl,
                    TrailerUrl = x.TrailerUrl,
                    ReleaseDate = x.ReleaseDate
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
                    Description = x.Description,
                    Language = x.Language,
                    DurationMinutes = x.DurationMinutes,
                    PosterUrl = x.PosterUrl,
                    TrailerUrl = x.TrailerUrl,
                    ReleaseDate = x.ReleaseDate
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

        public async Task<List<EventSelectDTO>> GetEventsByCityAsync(int cityId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = dbcontext.Events
                .AsNoTracking()
                .Where(e => e.IsActive)
                .Join(dbcontext.Shows,
                    e => e.Id,
                    s => s.EventId,
                    (e, s) => new { Event = e, Show = s })
                .Join(dbcontext.Venues,
                    es => es.Show.VenueId,
                    v => v.Id,
                    (es, v) => new { es.Event, es.Show, Venue = v })
                .Where(x => x.Venue.CityId == cityId);

            // Apply date filters if provided
            if (fromDate.HasValue)
            {
                query = query.Where(x => x.Show.ShowTime >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                query = query.Where(x => x.Show.ShowTime <= toDate.Value);
            }

            var results = await query
                .GroupBy(x => new
                {
                    x.Event.Id,
                    x.Event.Title,
                    x.Event.Description,
                    x.Event.Language,
                    x.Event.DurationMinutes,
                    x.Event.PosterUrl,
                    x.Event.TrailerUrl,
                    x.Event.ReleaseDate
                })
                .Select(g => new EventSelectDTO
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title,
                    Description = g.Key.Description,
                    Language = g.Key.Language,
                    DurationMinutes = g.Key.DurationMinutes,
                    PosterUrl = g.Key.PosterUrl,
                    TrailerUrl = g.Key.TrailerUrl,
                    ReleaseDate = g.Key.ReleaseDate,
                    CityIds = g.Select(x => x.Venue.CityId).Distinct().ToList(),
                    CityNames = g.Select(x => x.Venue.City.Name).Distinct().ToList(),
                    VenueNames = g.Select(x => x.Venue.Name).Distinct().ToList(),
                    UpcomingShowDates = g.Select(x => x.Show.ShowTime)
                        .Where(d => d >= DateTime.UtcNow)
                        .OrderBy(d => d)
                        .Take(3)
                        .ToList()
                })
                .ToListAsync();

            return results;
        }

        public async Task<List<EventSelectDTO>> GetNearbyEventsAsync(EventLocationQueryDto query)
        {
            // If cityId is provided, use city-based filtering
            if (query.CityId.HasValue)
            {
                return await GetEventsByCityAsync(query.CityId.Value, query.FromDate, query.ToDate);
            }

            // If coordinates are provided, use GPS-based filtering
            if (query.Latitude.HasValue && query.Longitude.HasValue)
            {
                var allEvents = await dbcontext.Events
                    .AsNoTracking()
                    .Where(e => e.IsActive)
                    .Join(dbcontext.Shows,
                        e => e.Id,
                        s => s.EventId,
                        (e, s) => new { Event = e, Show = s })
                    .Join(dbcontext.Venues,
                        es => es.Show.VenueId,
                        v => v.Id,
                        (es, v) => new { es.Event, es.Show, Venue = v })
                    .Where(x => x.Venue.Latitude != null && x.Venue.Longitude != null)
                    .ToListAsync();

                // Apply date filters
                if (query.FromDate.HasValue)
                {
                    allEvents = allEvents.Where(x => x.Show.ShowTime >= query.FromDate.Value).ToList();
                }

                if (query.ToDate.HasValue)
                {
                    allEvents = allEvents.Where(x => x.Show.ShowTime <= query.ToDate.Value).ToList();
                }

                // Calculate distances and filter by radius
                var eventsWithDistance = allEvents
                    .Select(x => new
                    {
                        x.Event,
                        x.Show,
                        x.Venue,
                        Distance = LocationHelper.CalculateDistance(
                            query.Latitude.Value,
                            query.Longitude.Value,
                            x.Venue.Latitude!.Value,
                            x.Venue.Longitude!.Value
                        )
                    })
                    .Where(x => x.Distance <= query.RadiusKm)
                    .ToList();

                // Group by event and create DTOs
                var results = eventsWithDistance
                    .GroupBy(x => new
                    {
                        x.Event.Id,
                        x.Event.Title,
                        x.Event.Description,
                        x.Event.Language,
                        x.Event.DurationMinutes,
                        x.Event.PosterUrl,
                        x.Event.TrailerUrl,
                        x.Event.ReleaseDate
                    })
                    .Select(g => new EventSelectDTO
                    {
                        Id = g.Key.Id,
                        Title = g.Key.Title,
                        Description = g.Key.Description,
                        Language = g.Key.Language,
                        DurationMinutes = g.Key.DurationMinutes,
                        PosterUrl = g.Key.PosterUrl,
                        TrailerUrl = g.Key.TrailerUrl,
                        ReleaseDate = g.Key.ReleaseDate,
                        CityIds = g.Select(x => x.Venue.CityId).Distinct().ToList(),
                        CityNames = g.Select(x => x.Venue.City.Name).Distinct().ToList(),
                        VenueNames = g.Select(x => x.Venue.Name).Distinct().ToList(),
                        DistanceKm = g.Min(x => x.Distance), // Closest venue
                        UpcomingShowDates = g.Select(x => x.Show.ShowTime)
                            .Where(d => d >= DateTime.UtcNow)
                            .OrderBy(d => d)
                            .Take(3)
                            .ToList()
                    })
                    .OrderBy(e => e.DistanceKm)
                    .Skip((query.PageNumber - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .ToList();

                return results;
            }

            // If neither cityId nor coordinates provided, return empty list
            return new List<EventSelectDTO>();
        }
    }
}
