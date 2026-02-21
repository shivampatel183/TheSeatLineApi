
using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data;
using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Business
{
    public class CityBusiness : ICityRepository
    {
        private readonly AppDbContext _context;

        public CityBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(City model)
        {
            await _context.Cities.AddAsync(model);
            await _context.SaveChangesAsync();
            return model.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _context.Cities.Where(c => c.Id == id).ExecuteDeleteAsync();

        }

        public async Task<List<CitySelectDto>> GetAllAsync()
        {
            return await _context.Cities
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new CitySelectDto
                {
                    Name = c.Name,
                    State = c.State,
                    Country = c.Country,
                })
                .ToListAsync();
        }

        public async Task UpdateAsync(City model)
        {
            var city = await _context.Cities
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            if (city == null)
                throw new Exception("City not found");

            city.Name = model.Name;
            city.State = model.State;
            city.Country = model.Country;
            city.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
        }
    }
}
