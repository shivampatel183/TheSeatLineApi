using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Business
{
    public class VenueBusiness : IVenueRepository
    {
        private readonly AppDbContext _context;

        public VenueBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VenueSummaryDto>> GetAllAsync()
        {
            return await _context.Venues
                .AsNoTracking()
                .Where(v => !v.IsDeleted)
                .Include(v => v.City)
                .OrderBy(v => v.Name)
                .Select(v => new VenueSummaryDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    City = v.City.Name,
                    State = v.City.State
                })
                .ToListAsync();
        }

        public async Task<Venue?> GetByIdAsync(Guid id)
        {
            return await _context.Venues
                .AsNoTracking()
                .Include(v => v.City)
                .Include(v => v.OperatingHours)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted);
        }

        public async Task<List<VenueSummaryDto>> GetByCityAsync(Guid cityId)
        {
            return await _context.Venues
                .AsNoTracking()
                .Where(v => v.CityId == cityId && !v.IsDeleted && v.IsActive)
                .Include(v => v.City)
                .OrderBy(v => v.Name)
                .Select(v => new VenueSummaryDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    City = v.City.Name,
                    State = v.City.State
                })
                .ToListAsync();
        }

        public async Task<Guid> CreateAsync(CreateVenueRequestDto dto)
        {
            var venue = new Venue
            {
                Name = dto.Name,
                VenueType = dto.VenueType,
                Description = dto.Description,
                AddressLine1 = dto.AddressLine1,
                CityId = dto.CityId,
                PostalCode = dto.PostalCode,
                Timezone = dto.Timezone,
                TotalCapacity = dto.TotalCapacity
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
            return venue.Id;
        }

        public async Task UpdateAsync(Guid id, CreateVenueRequestDto dto)
        {
            var venue = await _context.Venues
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted)
                ?? throw new Exception("Venue not found");

            venue.Name = dto.Name;
            venue.VenueType = dto.VenueType;
            venue.Description = dto.Description;
            venue.AddressLine1 = dto.AddressLine1;
            venue.CityId = dto.CityId;
            venue.PostalCode = dto.PostalCode;
            venue.Timezone = dto.Timezone;
            venue.TotalCapacity = dto.TotalCapacity;
            venue.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var venue = await _context.Venues
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted)
                ?? throw new Exception("Venue not found");

            venue.IsDeleted = true;
            venue.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
