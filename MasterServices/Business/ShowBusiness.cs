using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data;
using TheSeatLineApi.Entity;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Business
{
    public class ShowBusiness : IShowRepository
    {
        private readonly AppDbContext dbcontext;

        public ShowBusiness(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<List<ShowSelectDTO>> SelectShow()
        {
            return await dbcontext.Shows
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.Event)
                .Include(x => x.Venue)
                .Select(x => new ShowSelectDTO
                {
                    Id = x.Id,
                    EventId = x.EventId,
                    EventTitle = x.Event.Title,
                    VenueId = x.VenueId,
                    VenueName = x.Venue.Name,
                    ShowTime = x.ShowTime
                })
                .ToListAsync();
        }

        public async Task<List<ShowSelectDTO>> SelectShowByEvent(int eventId)
        {
            return await dbcontext.Shows
                .AsNoTracking()
                .Where(x => x.EventId == eventId && x.IsActive)
                .Include(x => x.Event)
                .Include(x => x.Venue)
                .Select(x => new ShowSelectDTO
                {
                    Id = x.Id,
                    EventId = x.EventId,
                    EventTitle = x.Event.Title,
                    VenueId = x.VenueId,
                    VenueName = x.Venue.Name,
                    ShowTime = x.ShowTime
                })
                .ToListAsync();
        }

        public async Task<List<ShowSelectDTO>> SelectShowByVenue(int venueId)
        {
            return await dbcontext.Shows
                .AsNoTracking()
                .Where(x => x.VenueId == venueId && x.IsActive)
                .Include(x => x.Event)
                .Include(x => x.Venue)
                .Select(x => new ShowSelectDTO
                {
                    Id = x.Id,
                    EventId = x.EventId,
                    EventTitle = x.Event.Title,
                    VenueId = x.VenueId,
                    VenueName = x.Venue.Name,
                    ShowTime = x.ShowTime
                })
                .ToListAsync();
        }

        public async Task<ShowSelectDTO?> SelectShowById(int id)
        {
            return await dbcontext.Shows
                .AsNoTracking()
                .Include(x => x.Event)
                .Include(x => x.Venue)
                .Where(x => x.Id == id)
                .Select(x => new ShowSelectDTO
                {
                    Id = x.Id,
                    EventId = x.EventId,
                    EventTitle = x.Event.Title,
                    VenueId = x.VenueId,
                    VenueName = x.Venue.Name,
                    ShowTime = x.ShowTime
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> InsertShow(ShowInsertDTO dto)
        {
            dbcontext.Shows.Add(new ShowEntity
            {
                EventId = dto.EventId,
                VenueId = dto.VenueId,
                ShowTime = dto.ShowTime,
                IsActive = true
            });

            return await dbcontext.SaveChangesAsync();
        }

        public async Task<int> UpdateShow(ShowInsertDTO dto)
        {
            return await dbcontext.Shows
                .Where(x => x.Id == dto.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.EventId, dto.EventId)
                    .SetProperty(p => p.VenueId, dto.VenueId)
                    .SetProperty(p => p.ShowTime, dto.ShowTime)
                    .SetProperty(p => p.IsActive, dto.IsActive)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow)
                );
        }

        public async Task<int> DeleteShow(int showId)
        {
            return await dbcontext.Shows
                .Where(x => x.Id == showId)
                .ExecuteDeleteAsync();
        }
    }
}
