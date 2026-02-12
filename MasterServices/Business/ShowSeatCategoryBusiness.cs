using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data;
using TheSeatLineApi.Entity;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Business
{
    public class ShowSeatCategoryBusiness : IShowSeatCategoryRepository
    {
        private readonly AppDbContext dbcontext;

        public ShowSeatCategoryBusiness(AppDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public async Task<List<ShowSeatCategorySelectDTO>> SelectShowSeatCategory(int showId)
        {
            return await dbcontext.ShowSeatCategories
                .AsNoTracking()
                .Where(x => x.ShowId == showId && x.IsActive)
                .Include(x => x.Show)
                .ThenInclude(s => s.Event)
                .Include(x => x.Show)
                .ThenInclude(s => s.Venue)
                .Select(x => new ShowSeatCategorySelectDTO
                {
                    Id = x.Id,
                    ShowId = x.ShowId,
                    EventTitle = x.Show.Event.Title,
                    VenueName = x.Show.Venue.Name,
                    ShowTime = x.Show.ShowTime,
                    SeatCategoryName = x.SeatCategoryName,
                    Price = x.Price,
                    AvailableSeats = x.AvailableSeats
                })
                .ToListAsync();
        }

        public async Task<int> InsertShowSeatCategory(ShowSeatCategoryInsertDTO dto)
        {
            dbcontext.ShowSeatCategories.Add(new ShowSeatCategoryEntity
            {
                ShowId = dto.ShowId,
                SeatCategoryName = dto.SeatCategoryName,
                Price = dto.Price,
                TotalSeats = dto.TotalSeats,
                AvailableSeats = dto.TotalSeats,
                IsActive = true
            });

            return await dbcontext.SaveChangesAsync();
        }

        public async Task<int> UpdateShowSeatCategory(ShowSeatCategoryInsertDTO dto)
        {
            return await dbcontext.ShowSeatCategories
                .Where(x => x.Id == dto.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.SeatCategoryName, dto.SeatCategoryName)
                    .SetProperty(p => p.Price, dto.Price)
                    .SetProperty(p => p.TotalSeats, dto.TotalSeats)
                    .SetProperty(p => p.AvailableSeats, dto.AvailableSeats)
                    .SetProperty(p => p.IsActive, dto.IsActive)
                    .SetProperty(p => p.UpdatedAt, DateTime.UtcNow)
                );
        }

        public async Task<int> DeleteShowSeatCategory(int id)
        {
            return await dbcontext.ShowSeatCategories
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }
    }
}
