namespace TheSeatLineApi.Modules.BookingModule.Repositories
{
    public interface ICouponRepository
    {
        Task<List<Coupon>> GetAllAsync();
        Task<Coupon?> GetByCodeAsync(string code);
        Task<Guid> CreateAsync(Coupon coupon);
        Task UpdateAsync(Guid id, Coupon coupon);
        Task DeleteAsync(Guid id);
        Task<decimal> ValidateAndApplyAsync(string code, decimal subtotal, Guid? eventId);
    }
}



