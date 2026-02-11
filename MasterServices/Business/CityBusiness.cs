using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Business
{
    public class CityBusiness : ICityRepository
    {
        private readonly AppDbContext _dbcontext;
        public CityBusiness(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public Task<int> DeleteCity(int cityId)
        {
            _dbcontext.Cities.Where(x => x.Id == cityId).ExecuteDelete();
            return _dbcontext.SaveChangesAsync();
        }

        public async Task<int> InsertCity(CityInsertDTO city)
        {
            _dbcontext.Cities.Add(new Entity.CityEntity
            {
                Name = city.Name,
                State = city.State,
                Country = city.Country,
                IsActive = true
            });

            return await _dbcontext.SaveChangesAsync();
        }

        public async Task<List<CitySelectDTO>> SelectCity()
        {
            return await _dbcontext.Cities
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => new CitySelectDTO
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
        }

        public Task<int> UpdateCity(CityInsertDTO city)
        {
            _dbcontext.Cities.Where(x => x.Id == city.Id).ExecuteUpdate(x => x
            .SetProperty(p => p.Name, city.Name)
            .SetProperty(p => p.State, city.State)
            .SetProperty(p => p.Country, city.Country)
            .SetProperty(p => p.IsActive, city.IsActive));
            return _dbcontext.SaveChangesAsync();
        }
    }
}
