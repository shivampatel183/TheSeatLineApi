using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.AuthServices.Helpers;
using TheSeatLineApi.BookingServices.DTOs;
using TheSeatLineApi.BookingServices.Repository;
using TheSeatLineApi.Common;

namespace TheSeatLineApi.BookingServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingService;

        public BookingController(IBookingRepository bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        public async Task<Response<BookingResponseDto>> Create(CreateBookingRequestDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                return Response<BookingResponseDto>.Ok(await _bookingService.CreateAsync(userId, dto), "Booking created — seats held for 10 minutes");
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(ex.Message);
            }
        }

        [HttpGet("my")]
        public async Task<Response<List<BookingResponseDto>>> GetMyBookings()
        {
            try
            {
                var userId = User.GetUserId();
                return Response<List<BookingResponseDto>>.Ok(await _bookingService.GetByUserAsync(userId));
            }
            catch (Exception ex)
            {
                return Response<List<BookingResponseDto>>.Fail(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<BookingResponseDto>> GetById(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var booking = await _bookingService.GetByIdAsync(id, userId)
                    ?? throw new Exception("Booking not found");
                return Response<BookingResponseDto>.Ok(booking);
            }
            catch (Exception ex)
            {
                return Response<BookingResponseDto>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}/cancel")]
        public async Task<Response<string>> Cancel(Guid id, [FromBody] string? reason)
        {
            try
            {
                var userId = User.GetUserId();
                await _bookingService.CancelAsync(id, userId, reason);
                return Response<string>.Ok(null, "Booking cancelled successfully");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}/transfer")]
        public async Task<Response<string>> Transfer(Guid id, TransferBookingRequestDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                await _bookingService.TransferAsync(id, userId, dto);
                return Response<string>.Ok(null, "Booking transferred successfully");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> UpdateStatus(Guid id, UpdateBookingStatusDto dto)
        {
            try
            {
                await _bookingService.UpdateStatusAsync(id, dto);
                return Response<string>.Ok(null, "Booking status updated");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}
