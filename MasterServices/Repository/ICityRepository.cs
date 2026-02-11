using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface ICityRepository
    {
        public Task<int> InsertCity(CityInsertDTO city);
        public Task<int> UpdateCity(CityInsertDTO city);
        public Task<int> DeleteCity(int cityId);
        public Task<List<CitySelectDTO>> SelectCity();
    }
}
