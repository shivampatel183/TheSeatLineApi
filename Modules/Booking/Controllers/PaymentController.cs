using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Modules.BookingModule.Models.DTOs;
using TheSeatLineApi.Modules.BookingModule.Repositories;
using TheSeatLineApi.Shared;

namespace TheSeatLineApi.Modules.BookingModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentService;

        public PaymentController(IPaymentRepository paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("create-order/{bookingId}")]
        public async Task<Response<Payment>> CreateOrder(Guid bookingId)
        {
            try
            {
                return Response<Payment>.Ok(await _paymentService.CreateOrderAsync(bookingId), "Payment order created");
            }
            catch (Exception ex)
            {
                return Response<Payment>.Fail(ex.Message);
            }
        }

        [HttpPost("verify")]
        public async Task<Response<Payment>> Verify(PaymentVerifyDto dto)
        {
            try
            {
                return Response<Payment>.Ok(await _paymentService.VerifyPaymentAsync(dto), "Payment verified successfully");
            }
            catch (Exception ex)
            {
                return Response<Payment>.Fail(ex.Message);
            }
        }

        [HttpPost("{id}/refund")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Payment>> Refund(Guid id, [FromBody] string reason)
        {
            try
            {
                return Response<Payment>.Ok(await _paymentService.RefundAsync(id, reason), "Refund processed");
            }
            catch (Exception ex)
            {
                return Response<Payment>.Fail(ex.Message);
            }
        }
    }
}



