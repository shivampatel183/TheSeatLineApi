using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.AuthServices.Helpers;
using TheSeatLineApi.Common;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewService;

        public ReviewController(IReviewRepository reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("event/{eventId}")]
        public async Task<Response<List<ReviewSelectDto>>> GetByEvent(Guid eventId)
        {
            try
            {
                return Response<List<ReviewSelectDto>>.Ok(await _reviewService.GetByEventAsync(eventId));
            }
            catch (Exception ex)
            {
                return Response<List<ReviewSelectDto>>.Fail(ex.Message);
            }
        }

        [HttpGet("venue/{venueId}")]
        public async Task<Response<List<ReviewSelectDto>>> GetByVenue(Guid venueId)
        {
            try
            {
                return Response<List<ReviewSelectDto>>.Ok(await _reviewService.GetByVenueAsync(venueId));
            }
            catch (Exception ex)
            {
                return Response<List<ReviewSelectDto>>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<Response<Guid>> Create(ReviewInsertDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                return Response<Guid>.Ok(await _reviewService.CreateAsync(userId, dto), "Review submitted");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<Response<string>> Delete(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                await _reviewService.DeleteAsync(id, userId);
                return Response<string>.Ok(null, "Review deleted");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}
