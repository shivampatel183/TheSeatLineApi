using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.AuthServices.Entity;
using TheSeatLineApi.AuthServices.Repository;
using TheSeatLineApi.Data;

namespace TheSeatLineApi.AuthServices.Business
{
    public class UserBusiness : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Guid> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user.Id;
        }
    }
}
