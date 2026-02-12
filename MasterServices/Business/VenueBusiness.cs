using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Entity;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Business
{
    public class VenueBusiness: IVenueRepository
    {
        private readonly AppDbContext _dbcontext;

        public VenueBusiness(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<int> InsertVenue(VenueInsertDTO venue)
        {
            _dbcontext.Venues.Add(new VenueEntity
            {
                Name = venue.Name,
                CityId = venue.CityId,
                Address = venue.Address,
                Latitude = venue.Latitude,
                Longitude = venue.Longitude,
                IsActive = true
            });

            return await _dbcontext.SaveChangesAsync();
        }

        public async Task<int> UpdateVenue(VenueInsertDTO venue)
        {
            return await _dbcontext.Venues
                .Where(x => x.Id == venue.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Name, venue.Name)
                    .SetProperty(p => p.CityId, venue.CityId)
                    .SetProperty(p => p.Address, venue.Address)
                    .SetProperty(p => p.Latitude, venue.Latitude)
                    .SetProperty(p => p.Longitude, venue.Longitude)
                    .SetProperty(p => p.IsActive, venue.IsActive)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow)
                );
        }

        public async Task<int> DeleteVenue(int venueId)
        {
            return await _dbcontext.Venues
                .Where(x => x.Id == venueId)
                .ExecuteDeleteAsync();
        }

        public async Task<List<VenueSelectDTO>> SelectVenue()
        {
            return await _dbcontext.Venues
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Include(x => x.City)
                .Select(x => new VenueSelectDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    CityId = x.CityId,
                    CityName = x.City.Name,
                    Address = x.Address
                })
                .ToListAsync();
        }

        public async Task<List<VenueSelectDTO>> SelectVenueByCity(int cityId)
        {
            return await _dbcontext.Venues
                .AsNoTracking()
                .Where(x => x.CityId == cityId && x.IsActive)
                .Include(x => x.City)
                .Select(x => new VenueSelectDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    CityId = x.CityId,
                    CityName = x.City.Name,
                    Address = x.Address
                })
                .ToListAsync();
        }

        public async Task<VenueSelectDTO?> SelectVenueById(int id)
        {
            return await _dbcontext.Venues
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.City)
                .Select(x => new VenueSelectDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    CityId = x.CityId,
                    CityName = x.City.Name,
                    Address = x.Address
                })
                .FirstOrDefaultAsync();
        }
    }
}

