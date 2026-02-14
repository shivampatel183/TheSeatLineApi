using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Common;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueRepository venueRepository;

        public VenueController(IVenueRepository venueRepository)
        {
            this.venueRepository = venueRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetVenue")]
        public async Task<Response<List<VenueSelectDTO>>> GetVenue()
        {
            try
            {
                return Response<List<VenueSelectDTO>>
                    .Ok(await venueRepository.SelectVenue());
            }
            catch (Exception ex)
            {
                return Response<List<VenueSelectDTO>>
                    .Fail(ex.Message);
            }
        }

        [HttpGet("GetVenueByCity")]
        public async Task<Response<List<VenueSelectDTO>>> GetVenueByCity(int cityId)
        {
            try
            {
                return Response<List<VenueSelectDTO>>
                    .Ok(await venueRepository.SelectVenueByCity(cityId));
            }
            catch (Exception ex)
            {
                return Response<List<VenueSelectDTO>>
                    .Fail(ex.Message);
            }
        }

        [HttpGet("GetVenueById")]
        public async Task<Response<VenueSelectDTO>> GetVenueById(int id)
        {
            try
            {
                return Response<VenueSelectDTO>
                    .Ok(await venueRepository.SelectVenueById(id));
            }
            catch (Exception ex)
            {
                return Response<VenueSelectDTO>
                    .Fail("Venue Not Found");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("InsertVenue")]
        public async Task<Response<int>> InsertVenue(VenueInsertDTO venue)
        {
            try
            {
                return Response<int>
                    .Ok(await venueRepository.InsertVenue(venue));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateVenue")]
        public async Task<Response<int>> UpdateVenue(VenueInsertDTO venue)
        {
            try
            {
                return Response<int>
                    .Ok(await venueRepository.UpdateVenue(venue));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteVenue")]
        public async Task<Response<int>> DeleteVenue(int venueId)
        {
            try
            {
                return Response<int>
                    .Ok(await venueRepository.DeleteVenue(venueId));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }
    }
}
