
namespace TheSeatLineApi.AuthServices.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<Guid> AddAsync(User user);
        Task<User> UpdateAsync(User user);
    }
}
