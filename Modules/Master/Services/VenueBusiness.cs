using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Data.Entities;
using TheSeatLineApi.Infrastructure.Persistence;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using TheSeatLineApi.Modules.MasterModule.Repositories;

namespace TheSeatLineApi.Modules.MasterModule.Services
{
    public class VenueBusiness : IVenueRepository
    {
        private readonly AppDbContext _context;

        public VenueBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<VenueListDto>> GetAllAsync()
        {
            return await _context.Venues
                .AsNoTracking()
                .Where(v => !v.IsDeleted && v.IsActive)
                .Include(v => v.City)
                .Select(v => new VenueListDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    CityName = v.City.Name,
                    AddressLine1 = v.AddressLine1,
                    IsActive = v.IsActive
                })
                .ToListAsync();
        }

        public async Task<VenueDetailDto?> GetByIdAsync(Guid id)
        {
            // 1. Fetch the venue entity (and related data) from the database
            var venue = await _context.Venues
                .AsNoTracking()
                .Where(v => v.Id == id && !v.IsDeleted)
                .Include(v => v.City)
                .Include(v => v.OperatingHours)
                .FirstOrDefaultAsync();

            if (venue == null)
                return null;

            // 2. Map to DTO in memory (after the database query)
            return new VenueDetailDto
            {
                Id = venue.Id,
                Name = venue.Name,
                CityName = venue.City.Name,
                AddressLine1 = venue.AddressLine1,
                AddressLine2 = venue.AddressLine2,
                PostalCode = venue.PostalCode,
                Latitude = venue.Latitude,
                Longitude = venue.Longitude,
                Timezone = venue.Timezone,
                TotalCapacity = venue.TotalCapacity,
                Amenities = venue.Amenities != null
                    ? JsonSerializer.Deserialize<List<string>>(venue.Amenities)
                    : null,
                AccessibilityFeatures = venue.AccessibilityFeatures != null
                    ? JsonSerializer.Deserialize<List<string>>(venue.AccessibilityFeatures)
                    : null,
                MediaGallery = venue.MediaGallery != null
                    ? JsonSerializer.Deserialize<List<string>>(venue.MediaGallery)
                    : null,
                ContactEmail = venue.ContactEmail,
                ContactPhone = venue.ContactPhone,
                WebsiteUrl = venue.WebsiteUrl,
                IsActive = venue.IsActive,
            };
        }

        public async Task<Guid> CreateAsync(VenueCreateDto dto)
        {
            var venue = new Venue
            {
                CityId = dto.CityId,
                Name = dto.Name,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                PostalCode = dto.PostalCode,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Timezone = dto.Timezone,
                TotalCapacity = dto.TotalCapacity,
                Amenities = dto.Amenities != null ? JsonSerializer.Serialize(dto.Amenities) : null,
                AccessibilityFeatures = dto.AccessibilityFeatures != null ? JsonSerializer.Serialize(dto.AccessibilityFeatures) : null,
                MediaGallery = dto.MediaGallery != null ? JsonSerializer.Serialize(dto.MediaGallery) : null,
                ContactEmail = dto.ContactEmail,
                ContactPhone = dto.ContactPhone,
                WebsiteUrl = dto.WebsiteUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();

            // Add operating hours
            if (dto.OperatingHours?.Any() == true)
            {
                var hours = dto.OperatingHours.Select(oh => new VenueOperatingHours
                {
                    VenueId = venue.Id,
                    DayOfWeek = oh.DayOfWeek,
                    OpenTime = oh.OpenTime,
                    CloseTime = oh.CloseTime,
                    IsClosed = oh.IsClosed
                });
                _context.VenueOperatingHours.AddRange(hours);
                await _context.SaveChangesAsync();
            }

            return venue.Id;
        }

        public async Task UpdateAsync(Guid id, VenueUpdateDto dto)
        {
            var venue = await _context.Venues
                .Include(v => v.OperatingHours)
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted)
                ?? throw new Exception("Venue not found");

            // Update basic fields
            venue.CityId = dto.CityId;
            venue.Name = dto.Name;
            venue.AddressLine1 = dto.AddressLine1;
            venue.AddressLine2 = dto.AddressLine2;
            venue.PostalCode = dto.PostalCode;
            venue.Latitude = dto.Latitude;
            venue.Longitude = dto.Longitude;
            venue.Timezone = dto.Timezone;
            venue.TotalCapacity = dto.TotalCapacity;
            venue.Amenities = dto.Amenities != null ? JsonSerializer.Serialize(dto.Amenities) : null;
            venue.AccessibilityFeatures = dto.AccessibilityFeatures != null ? JsonSerializer.Serialize(dto.AccessibilityFeatures) : null;
            venue.MediaGallery = dto.MediaGallery != null ? JsonSerializer.Serialize(dto.MediaGallery) : null;
            venue.ContactEmail = dto.ContactEmail;
            venue.ContactPhone = dto.ContactPhone;
            venue.WebsiteUrl = dto.WebsiteUrl;
            venue.UpdatedAt = DateTime.UtcNow;

            // Replace operating hours
            _context.VenueOperatingHours.RemoveRange(venue.OperatingHours);
            if (dto.OperatingHours?.Any() == true)
            {
                var newHours = dto.OperatingHours.Select(oh => new VenueOperatingHours
                {
                    VenueId = venue.Id,
                    DayOfWeek = oh.DayOfWeek,
                    OpenTime = oh.OpenTime,
                    CloseTime = oh.CloseTime,
                    IsClosed = oh.IsClosed
                });
                await _context.VenueOperatingHours.AddRangeAsync(newHours);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var venue = await _context.Venues
                .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted)
                ?? throw new Exception("Venue not found");

            venue.IsDeleted = true;
            venue.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}



