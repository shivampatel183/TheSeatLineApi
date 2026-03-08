using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Shared.Helpers;
using TheSeatLineApi.Infrastructure.Persistence;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using TheSeatLineApi.Modules.MasterModule.Repositories;

namespace TheSeatLineApi.Modules.MasterModule.Services
{
    public class EventBusiness : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventBusiness(AppDbContext context)
        {
            _context = context;
        }

        // ==================== Public listing ====================
        public async Task<List<EventSelectDTO>> GetAllAsync(EventLocationQueryDto? query = null)
        {
            var q = _context.Events
                .AsNoTracking()
                .Where(e => !e.IsDeleted && e.Status == 1)  // only published events for public
                .Include(e => e.Venue)
                    .ThenInclude(v => v.City)
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                .AsQueryable();

            // Filter by city slug
            if (!string.IsNullOrWhiteSpace(query?.CitySlug))
            {
                q = q.Where(e => e.Venue.City.Slug == query.CitySlug);
            }

            // Filter by date range
            if (query?.FromDate != null)
                q = q.Where(e => e.StartDateTime >= query.FromDate);
            if (query?.ToDate != null)
                q = q.Where(e => e.EndDateTime <= query.ToDate);

            // Filter by category
            if (query?.CategoryId != null)
                q = q.Where(e => e.CategoryId == query.CategoryId);

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
                    EndDateTime = (DateTime)e.EndDateTime,
                    BannerImageUrl = e.BannerImageUrl,
                    Status = e.Status,
                    VenueId = e.VenueId,
                    VenueName = e.Venue.Name,
                    City = e.Venue.City.Name,
                    CitySlug = e.Venue.City.Slug,
                    State = e.Venue.City.State,
                    CategoryName = e.Category != null ? e.Category.Name : null,
                    Tags = e.EventTags.Select(t => t.Tag).ToList()
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

        // ==================== Get by ID (public details) ====================
        public async Task<EventDetailDTO?> GetByIdAsync(Guid id)
        {
            //var currentUserId = _currentUserService.UserId;
            var query = _context.Events
                .AsNoTracking()
                .Where(e => e.Id == id && !e.IsDeleted)
                .Include(e => e.Venue)
                    .ThenInclude(v => v.City)
                .Include(e => e.Category)
                .Include(e => e.EventTags)
                .Include(e => e.Images.OrderBy(i => i.SortOrder))
                .Include(e => e.Shows);

            //// If user is not authenticated or not the organizer, only show published events
            //if (currentUserId == null)
            //{
            //    query = query.Where(e => e.Status == 1); // published only
            //}
            //else
            //{
            //    // Allow organizer to see their own events regardless of status
            //    query = query.Where(e => e.Status == 1 || e.OrganizerId == currentUserId);
            //}

            return await query
                .Select(e => new EventDetailDTO
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    EventType = e.EventType,
                    Language = e.Language,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = (DateTime)e.EndDateTime,
                    BannerImageUrl = e.BannerImageUrl,
                    Status = e.Status,
                    VenueId = e.VenueId,
                    VenueName = e.Venue.Name,
                    City = e.Venue.City.Name,
                    CitySlug = e.Venue.City.Slug,
                    State = e.Venue.City.State,
                    CategoryName = e.Category != null ? e.Category.Name : null,
                    Tags = e.EventTags.Select(t => t.Tag).ToList(),
                    Performers = e.Performers,
                    AgeRestriction = e.AgeRestriction,
                    Timezone = e.Timezone,
                    MaxCapacity = e.MaxCapacity,
                    IsRecurring = e.IsRecurring,
                    RecurrenceRule = e.RecurrenceRule,
                    Images = e.Images.Select(i => new EventImageDto
                    {
                        ImageUrl = i.ImageUrl,
                        SortOrder = i.SortOrder
                    }).ToList(),
                    Shows = e.Shows.Select(s => new EventShowDto
                    {
                        Id = s.Id,
                        StartDateTime = s.StartDateTime,
                        EndDateTime = s.EndDateTime,
                        Status = s.Status,
                        // AvailableSeats = s.MaxCapacity - s.Tickets.Count(t => t.Status != 3) // optional
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        // ==================== Create (organizer only) ====================
        public async Task<Guid> CreateAsync(EventInsertDTO dto, Guid currentUserId)
        {
            //var currentUserId = _currentUserService.UserId
                //?? throw new UnauthorizedAccessException("User must be authenticated to create an event.");

            // Optionally check role: if (!_currentUserService.IsOrganizer) throw...

            // Generate unique slug
            var baseSlug = dto.Title.ToLower().Replace(" ", "-");
            var slug = baseSlug + "-" + Guid.NewGuid().ToString("N")[..8];

            var ev = new Event
            {
                //OrganizerId = currentUserId,                  // Link to the creating user
                VenueId = dto.VenueId,
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Slug = slug,
                Description = dto.Description,
                EventType = dto.EventType,
                StartDateTime = dto.StartDateTime,
                EndDateTime = dto.EndDateTime,
                Timezone = dto.Timezone,
                Language = dto.Language,
                MaxCapacity = dto.MaxCapacity,
                BannerImageUrl = dto.BannerImageUrl,
                Performers = dto.Performers,
                Status = 0,                                     // Draft by default
                CreatedBy = currentUserId,
                UpdatedBy = currentUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            // Add tags
            if (dto.Tags?.Any() == true)
            {
                var tags = dto.Tags.Select(t => new EventTag
                {
                    EventId = ev.Id,
                    Tag = t
                });
                _context.EventTags.AddRange(tags);
                await _context.SaveChangesAsync();
            }

            return ev.Id;
        }

        // ==================== Update (organizer only) ====================
        public async Task UpdateAsync(Guid id, EventInsertDTO dto, Guid currentUserId)
        {
            //var currentUserId = _currentUserService.UserId
            //    ?? throw new UnauthorizedAccessException("User must be authenticated to update an event.");

            var ev = await _context.Events
                .Include(e => e.EventTags)
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted)
                ?? throw new Exception("Event not found");

            // Ensure the user is the organizer (or admin)
            //if (ev.OrganizerId != currentUserId && !_currentUserService.IsAdmin)
            //    throw new UnauthorizedAccessException("You are not authorized to update this event.");

            ev.Title = dto.Title;
            ev.VenueId = dto.VenueId;
            ev.CategoryId = dto.CategoryId;
            ev.Description = dto.Description;
            ev.EventType = dto.EventType;
            ev.StartDateTime = dto.StartDateTime;
            ev.EndDateTime = dto.EndDateTime;
            ev.Timezone = dto.Timezone;
            ev.Language = dto.Language;
            ev.MaxCapacity = dto.MaxCapacity;
            ev.BannerImageUrl = dto.BannerImageUrl;
            ev.Performers = dto.Performers;
            ev.UpdatedBy = currentUserId;
            ev.UpdatedAt = DateTime.UtcNow;

            // Update tags: remove old, add new
            _context.EventTags.RemoveRange(ev.EventTags);
            if (dto.Tags?.Any() == true)
            {
                var newTags = dto.Tags.Select(t => new EventTag
                {
                    EventId = ev.Id,
                    Tag = t
                });
                await _context.EventTags.AddRangeAsync(newTags);
            }

            await _context.SaveChangesAsync();
        }

        // ==================== Delete (organizer only) ====================
        public async Task DeleteAsync(Guid id, Guid currentUserId)
        {
            //var currentUserId = _currentUserService.UserId
            //    ?? throw new UnauthorizedAccessException("User must be authenticated to delete an event.");

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted)
                ?? throw new Exception("Event not found");

            //if (ev.OrganizerId != currentUserId && !_currentUserService.IsAdmin)
            //    throw new UnauthorizedAccessException("You are not authorized to delete this event.");

            ev.IsDeleted = true;
            ev.UpdatedAt = DateTime.UtcNow;
            ev.UpdatedBy = currentUserId;
            await _context.SaveChangesAsync();
        }
    }
}


