using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Infrastructure.Persistence;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using TheSeatLineApi.Modules.MasterModule.Repositories;

namespace TheSeatLineApi.Modules.MasterModule.Services
{
    public class ReviewBusiness : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReviewSelectDto>> GetByEventAsync(Guid eventId)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(r => r.EventId == eventId && !r.IsDeleted)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewSelectDto
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    VenueId = r.VenueId,
                    UserId = r.UserId,
                    UserName = r.User.FirstName + " " + r.User.LastName,
                    Rating = r.Rating,
                    Title = r.Title,
                    Body = r.Body,
                    IsVerified = r.IsVerified,
                    HelpfulVotes = r.HelpfulVotes,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<ReviewSelectDto>> GetByVenueAsync(Guid venueId)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Where(r => r.VenueId == venueId && !r.IsDeleted)
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new ReviewSelectDto
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    VenueId = r.VenueId,
                    UserId = r.UserId,
                    UserName = r.User.FirstName + " " + r.User.LastName,
                    Rating = r.Rating,
                    Title = r.Title,
                    Body = r.Body,
                    IsVerified = r.IsVerified,
                    HelpfulVotes = r.HelpfulVotes,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<Guid> CreateAsync(Guid userId, ReviewInsertDto dto)
        {
            var review = new Review
            {
                EventId = dto.EventId,
                VenueId = dto.VenueId,
                UserId = userId,
                BookingId = dto.BookingId,
                Rating = dto.Rating,
                Title = dto.Title,
                Body = dto.Body,
                IsVerified = true
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review.Id;
        }

        public async Task DeleteAsync(Guid id, Guid userId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId && !r.IsDeleted)
                ?? throw new Exception("Review not found");

            review.IsDeleted = true;
            review.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}



