using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface ICityRepository
    {
        Task<int> InsertCity(CityInsertDTO city);
        Task<int> UpdateCity(CityInsertDTO city);
        Task<int> DeleteCity(int cityId);
        Task<List<CitySelectDTO>> SelectCity();
        Task<CitySelectDTO?> SelectCityByName(string name);
        Task<CitySelectDTO?> SelectCityById(int id);
    }
}
