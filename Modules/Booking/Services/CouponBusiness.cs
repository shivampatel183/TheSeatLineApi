using Microsoft.EntityFrameworkCore;
using TheSeatLineApi.Modules.BookingModule.Repositories;
using TheSeatLineApi.Infrastructure.Persistence;

namespace TheSeatLineApi.Modules.BookingModule.Services
{
    public class CouponBusiness : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponBusiness(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Coupon>> GetAllAsync()
        {
            return await _context.Coupons.AsNoTracking().OrderBy(c => c.Code).ToListAsync();
        }

        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            return await _context.Coupons.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
        }

        public async Task<Guid> CreateAsync(Coupon coupon)
        {
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
            return coupon.Id;
        }

        public async Task UpdateAsync(Guid id, Coupon coupon)
        {
            var existing = await _context.Coupons.FindAsync(id)
                ?? throw new Exception("Coupon not found");

            existing.Code = coupon.Code;
            existing.Description = coupon.Description;
            existing.DiscountType = coupon.DiscountType;
            existing.DiscountValue = coupon.DiscountValue;
            existing.MaxDiscount = coupon.MaxDiscount;
            existing.MinPurchaseAmt = coupon.MinPurchaseAmt;
            existing.ValidFrom = coupon.ValidFrom;
            existing.ValidUntil = coupon.ValidUntil;
            existing.TotalUsageLimit = coupon.TotalUsageLimit;
            existing.PerUserLimit = coupon.PerUserLimit;
            existing.IsActive = coupon.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var coupon = await _context.Coupons.FindAsync(id)
                ?? throw new Exception("Coupon not found");

            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> ValidateAndApplyAsync(string code, decimal subtotal, Guid? eventId)
        {
            var coupon = await _context.Coupons
                .FirstOrDefaultAsync(c => c.Code == code && c.IsActive)
                ?? throw new Exception("Invalid coupon code");

            if (DateTime.UtcNow < coupon.ValidFrom || DateTime.UtcNow > coupon.ValidUntil)
                throw new Exception("Coupon has expired");

            if (coupon.TotalUsageLimit.HasValue && coupon.UsedCount >= coupon.TotalUsageLimit.Value)
                throw new Exception("Coupon usage limit reached");

            if (coupon.MinPurchaseAmt.HasValue && subtotal < coupon.MinPurchaseAmt.Value)
                throw new Exception($"Minimum purchase amount of {coupon.MinPurchaseAmt} required");

            // Calculate discount
            decimal discount;
            if (coupon.DiscountType == 1) // Percentage
            {
                discount = Math.Round(subtotal * coupon.DiscountValue / 100, 2);
                if (coupon.MaxDiscount.HasValue)
                    discount = Math.Min(discount, coupon.MaxDiscount.Value);
            }
            else // Flat
            {
                discount = coupon.DiscountValue;
            }

            // Increment usage
            coupon.UsedCount++;
            await _context.SaveChangesAsync();

            return discount;
        }
    }
}



