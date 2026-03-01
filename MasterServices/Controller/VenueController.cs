using Microsoft.AspNetCore.Authorization;
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
        private readonly IVenueRepository _venueService;

        public VenueController(IVenueRepository venueService)
        {
            _venueService = venueService;
        }

        [HttpGet]
        public async Task<Response<List<VenueSummaryDto>>> GetAll()
        {
            try
            {
                return Response<List<VenueSummaryDto>>.Ok(await _venueService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return Response<List<VenueSummaryDto>>.Fail(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<Venue>> GetById(Guid id)
        {
            try
            {
                var venue = await _venueService.GetByIdAsync(id)
                    ?? throw new Exception("Venue not found");
                return Response<Venue>.Ok(venue);
            }
            catch (Exception ex)
            {
                return Response<Venue>.Fail(ex.Message);
            }
        }

        [HttpGet("city/{cityId}")]
        public async Task<Response<List<VenueSummaryDto>>> GetByCity(Guid cityId)
        {
            try
            {
                return Response<List<VenueSummaryDto>>.Ok(await _venueService.GetByCityAsync(cityId));
            }
            catch (Exception ex)
            {
                return Response<List<VenueSummaryDto>>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Guid>> Create(CreateVenueRequestDto dto)
        {
            try
            {
                return Response<Guid>.Ok(await _venueService.CreateAsync(dto), "Venue created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Update(Guid id, CreateVenueRequestDto dto)
        {
            try
            {
                await _venueService.UpdateAsync(id, dto);
                return Response<string>.Ok(null, "Venue updated successfully");
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
                await _venueService.DeleteAsync(id);
                return Response<string>.Ok(null, "Venue deleted successfully");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}
