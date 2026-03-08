using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Modules.BookingModule.Repositories;
using TheSeatLineApi.Shared;

namespace TheSeatLineApi.Modules.BookingModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponRepository _couponService;

        public CouponController(ICouponRepository couponService)
        {
            _couponService = couponService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<Response<List<Coupon>>> GetAll()
        {
            try
            {
                return Response<List<Coupon>>.Ok(await _couponService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return Response<List<Coupon>>.Fail(ex.Message);
            }
        }

        [HttpGet("{code}")]
        [Authorize]
        public async Task<Response<Coupon>> GetByCode(string code)
        {
            try
            {
                var coupon = await _couponService.GetByCodeAsync(code)
                    ?? throw new Exception("Coupon not found");
                return Response<Coupon>.Ok(coupon);
            }
            catch (Exception ex)
            {
                return Response<Coupon>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Guid>> Create(Coupon coupon)
        {
            try
            {
                return Response<Guid>.Ok(await _couponService.CreateAsync(coupon), "Coupon created");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Update(Guid id, Coupon coupon)
        {
            try
            {
                await _couponService.UpdateAsync(id, coupon);
                return Response<string>.Ok(null, "Coupon updated");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Delete(Guid id)
        {
            try
            {
                await _couponService.DeleteAsync(id);
                return Response<string>.Ok(null, "Coupon deleted");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }

        [HttpPost("validate")]
        [Authorize]
        public async Task<Response<decimal>> Validate([FromQuery] string code, [FromQuery] decimal subtotal, [FromQuery] Guid? eventId)
        {
            try
            {
                var discount = await _couponService.ValidateAndApplyAsync(code, subtotal, eventId);
                return Response<decimal>.Ok(discount, $"Coupon applied â€” discount: {discount}");
            }
            catch (Exception ex)
            {
                return Response<decimal>.Fail(ex.Message);
            }
        }
    }
}



