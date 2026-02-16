using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheSeatLineApi.BookingServices.Business;
using TheSeatLineApi.BookingServices.DTOs;
using TheSeatLineApi.Common;

namespace TheSeatLineApi.BookingServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingBusiness _bookingBusiness;

        public BookingController(IBookingBusiness bookingBusiness)
        {
            _bookingBusiness = bookingBusiness;
        }

        private Guid GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.Parse(userIdClaim ?? throw new UnauthorizedAccessException("User not authenticated"));
        }

        [HttpPost]
        public async Task<Response<BookingResponseDto>> CreateBooking([FromBody] CreateBookingRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                return await _bookingBusiness.CreateBookingAsync(userId, request);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<BookingResponseDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<BookingResponseDto>> GetBooking(Guid id)
        {
            try
            {
                var userId = GetUserId();
                return await _bookingBusiness.GetBookingDetailsAsync(id, userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<BookingResponseDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("user")]
        public async Task<Response<List<BookingResponseDto>>> GetUserBookings()
        {
            try
            {
                var userId = GetUserId();
                return await _bookingBusiness.GetUserBookingsAsync(userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<List<BookingResponseDto>>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Response<List<BookingResponseDto>>.Fail($"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}/confirm")]
        public async Task<Response<BookingResponseDto>> ConfirmBooking(Guid id)
        {
            try
            {
                var userId = GetUserId();
                return await _bookingBusiness.ConfirmBookingAsync(id, userId);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<BookingResponseDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<Response<BookingResponseDto>> CancelBooking(Guid id, [FromBody] string? cancellationReason = null)
        {
            try
            {
                var userId = GetUserId();
                return await _bookingBusiness.CancelBookingAsync(id, userId, cancellationReason);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<BookingResponseDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail($"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}/transfer")]
        public async Task<Response<BookingResponseDto>> TransferBooking(Guid id, [FromBody] TransferBookingRequestDto request)
        {
            try
            {
                var userId = GetUserId();
                return await _bookingBusiness.TransferBookingAsync(id, userId, request);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Response<BookingResponseDto>.Fail(ex.Message);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail($"An error occurred: {ex.Message}");
            }
        }
    }
}

