using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Shared;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using TheSeatLineApi.Modules.MasterModule.Repositories;

namespace TheSeatLineApi.Modules.MasterModule.Controllers
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
        public async Task<Response<List<VenueListDto>>> GetAll()
        {
            try
            {
                return Response<List<VenueListDto>>.Ok(await _venueService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return Response<List<VenueListDto>>.Fail(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<VenueDetailDto>> GetById(Guid id)
        {
            try
            {
                var venue = await _venueService.GetByIdAsync(id);
                return venue == null
                    ? Response<VenueDetailDto>.Fail("Venue not found")
                    : Response<VenueDetailDto>.Ok(venue);
            }
            catch (Exception ex)
            {
                return Response<VenueDetailDto>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Guid>> Create(VenueCreateDto dto)
        {
            try
            {
                return Response<Guid>.Ok(await _venueService.CreateAsync(dto), "Venue created");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Update(Guid id, VenueUpdateDto dto)
        {
            try
            {
                await _venueService.UpdateAsync(id, dto);
                return Response<string>.Ok(null, "Venue updated");
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
                return Response<string>.Ok(null, "Venue deleted");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}



