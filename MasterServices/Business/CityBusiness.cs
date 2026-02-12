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

        public async Task<int> DeleteCity(int cityId)
        {
            return await _dbcontext.Cities
                .Where(x => x.Id == cityId)
                .ExecuteDeleteAsync();
        }

        public async Task<int> InsertCity(CityInsertDTO city)
        {
            
            _dbcontext.Cities.Add(new Entity.CityEntity
            {
                Name = city.Name,
                State = city.State,
                Country = city.Country,
                Slug = city.Slug,
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
                Name = x.Name,
                Slug = x.Slug
            })
            .ToListAsync();
        }

        public async Task<int> UpdateCity(CityInsertDTO city)
        {
            return await _dbcontext.Cities
                .Where(x => x.Id == city.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.Name, city.Name)
                    .SetProperty(p => p.State, city.State)
                    .SetProperty(p => p.Country, city.Country)
                    .SetProperty(p => p.IsActive, city.IsActive)
                    .SetProperty(p => p.Slug, city.Slug)
                );
        }


        public async Task<CitySelectDTO?> SelectCityByName(string name)
        {
            return await _dbcontext.Cities
                .AsNoTracking()
                .Where(x => x.Name == name)
                .Select(x => new CitySelectDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug
                })
                .FirstOrDefaultAsync();
        }

        public async Task<CitySelectDTO?> SelectCityById(int id)
        {
            return await _dbcontext.Cities
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new CitySelectDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Slug = x.Slug
                })
                .FirstOrDefaultAsync();
        }

    }
}
