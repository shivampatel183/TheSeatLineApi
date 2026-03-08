using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Infrastructure.Persistence;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using TheSeatLineApi.Modules.MasterModule.Repositories;

namespace TheSeatLineApi.Modules.MasterModule.Services
{
    public class SeatBusiness : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SeatSelectDTO>> GetByEventAsync(Guid eventId)
        {
            return await _context.Seats
                .AsNoTracking()
                .Where(s => s.EventId == eventId)
                .OrderBy(s => s.Section)
                .ThenBy(s => s.Row)
                .ThenBy(s => s.SeatNumber)
                .Select(s => new SeatSelectDTO
                {
                    Id = s.Id,
                    EventId = s.EventId,
                    Section = s.Section,
                    Row = s.Row,
                    SeatNumber = s.SeatNumber,
                    SeatType = s.SeatType,
                    Status = s.Status,
                    BasePrice = s.BasePrice,
                    Currency = s.Currency,
                    IsAisle = s.IsAisle,
                    IsWindow = s.IsWindow,
                    IsEmergencyExit = s.IsEmergencyExit
                })
                .ToListAsync();
        }

        public async Task<Guid> CreateAsync(SeatInsertDTO dto)
        {
            var seat = new Seat
            {
                EventId = dto.EventId,
                Section = dto.Section,
                Row = dto.Row,
                SeatNumber = dto.SeatNumber,
                SeatType = dto.SeatType,
                BasePrice = dto.BasePrice,
                IsAisle = dto.IsAisle,
                IsWindow = dto.IsWindow,
                IsEmergencyExit = dto.IsEmergencyExit
            };

            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            return seat.Id;
        }

        public async Task<List<Guid>> BulkCreateAsync(SeatBulkInsertDTO dto)
        {
            var seats = new List<Seat>();

            for (int i = dto.StartNumber; i <= dto.EndNumber; i++)
            {
                seats.Add(new Seat
                {
                    EventId = dto.EventId,
                    Section = dto.Section,
                    Row = dto.RowLabel,
                    SeatNumber = i.ToString(),
                    SeatType = dto.SeatType,
                    BasePrice = dto.BasePrice
                });
            }

            _context.Seats.AddRange(seats);
            await _context.SaveChangesAsync();
            return seats.Select(s => s.Id).ToList();
        }

        public async Task UpdateAsync(Guid id, SeatInsertDTO dto)
        {
            var seat = await _context.Seats.FindAsync(id)
                ?? throw new Exception("Seat not found");

            seat.Section = dto.Section;
            seat.Row = dto.Row;
            seat.SeatNumber = dto.SeatNumber;
            seat.SeatType = dto.SeatType;
            seat.BasePrice = dto.BasePrice;
            seat.IsAisle = dto.IsAisle;
            seat.IsWindow = dto.IsWindow;
            seat.IsEmergencyExit = dto.IsEmergencyExit;
            seat.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var seat = await _context.Seats.FindAsync(id)
                ?? throw new Exception("Seat not found");

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
        }
    }
}



