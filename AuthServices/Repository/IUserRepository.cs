using TheSeatLineApi.AuthServices.Entity;

namespace TheSeatLineApi.AuthServices.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<Guid> AddAsync(User user);
    }
}
