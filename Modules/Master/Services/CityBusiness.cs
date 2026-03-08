
using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Infrastructure.Persistence;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

namespace TheSeatLineApi.Modules.MasterModule.Services
{
    public class CityBusiness : ICityRepository
    {
        private readonly AppDbContext _context;

        public CityBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CityListDto>> GetAllAsync()
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.IsActive)
                .Select(c => new CityListDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    State = c.State,
                    Country = c.Country,
                    Slug = c.Slug,
                    IsActive = c.IsActive
                })
                .ToListAsync();
        }

        public async Task<CityDetailDto?> GetByIdAsync(Guid id)
        {
            return await _context.Cities
                .AsNoTracking()
                .Where(c => c.Id == id && c.IsActive)
                .Select(c => new CityDetailDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    State = c.State,
                    Country = c.Country,
                    Slug = c.Slug,
                    IsActive = c.IsActive
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Guid> CreateAsync(CityCreateDto dto)
        {            

            var city = new City
            {
                Name = dto.Name,
                State = dto.State,
                Country = dto.Country,
                Slug = dto.Slug,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
            return city.Id;
        }

        public async Task UpdateAsync(Guid id, CityUpdateDto dto)
        {

            var city = await _context.Cities.FindAsync(id)
                ?? throw new Exception("City not found");

            city.Name = dto.Name;
            city.State = dto.State;
            city.Country = dto.Country;
            city.Slug = dto.Slug;
            city.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {

            var city = await _context.Cities.FindAsync(id)
                ?? throw new Exception("City not found");

            city.IsActive = false; // soft delete
            await _context.SaveChangesAsync();
        }
    }
}



