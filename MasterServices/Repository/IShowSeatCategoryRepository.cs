using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Repository
{
    public interface IShowSeatCategoryRepository
    {
        Task<List<ShowSeatCategorySelectDTO>> SelectShowSeatCategory(int showId);

        Task<int> InsertShowSeatCategory(ShowSeatCategoryInsertDTO dto);

        Task<int> UpdateShowSeatCategory(ShowSeatCategoryInsertDTO dto);

        Task<int> DeleteShowSeatCategory(int id);
    }
}
