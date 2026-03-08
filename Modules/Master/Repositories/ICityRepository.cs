using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

public interface ICityRepository
{
    Task<List<CityListDto>> GetAllAsync();
    Task<CityDetailDto?> GetByIdAsync(Guid id);
    Task<Guid> CreateAsync(CityCreateDto dto);
    Task UpdateAsync(Guid id, CityUpdateDto dto);
    Task DeleteAsync(Guid id);
}