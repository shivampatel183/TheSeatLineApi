using TheSeatLineApi.MasterServices.DTOs;

public interface ICityRepository
{
    Task<List<CitySelectDto>> GetAllAsync();
    Task<Guid> CreateAsync(City model);
    Task UpdateAsync(City model);
    Task DeleteAsync(Guid id);
}