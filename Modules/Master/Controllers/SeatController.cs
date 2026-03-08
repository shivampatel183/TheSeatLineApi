using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Shared;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using TheSeatLineApi.Modules.MasterModule.Repositories;

namespace TheSeatLineApi.Modules.MasterModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatController : ControllerBase
    {
        private readonly ISeatRepository _seatService;

        public SeatController(ISeatRepository seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("event/{eventId}")]
        public async Task<Response<List<SeatSelectDTO>>> GetByEvent(Guid eventId)
        {
            try
            {
                return Response<List<SeatSelectDTO>>.Ok(await _seatService.GetByEventAsync(eventId));
            }
            catch (Exception ex)
            {
                return Response<List<SeatSelectDTO>>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Guid>> Create(SeatInsertDTO dto)
        {
            try
            {
                return Response<Guid>.Ok(await _seatService.CreateAsync(dto), "Seat created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpPost("bulk")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<List<Guid>>> BulkCreate(SeatBulkInsertDTO dto)
        {
            try
            {
                return Response<List<Guid>>.Ok(await _seatService.BulkCreateAsync(dto), "Seats created successfully");
            }
            catch (Exception ex)
            {
                return Response<List<Guid>>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Update(Guid id, SeatInsertDTO dto)
        {
            try
            {
                await _seatService.UpdateAsync(id, dto);
                return Response<string>.Ok(null, "Seat updated successfully");
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
                await _seatService.DeleteAsync(id);
                return Response<string>.Ok(null, "Seat deleted successfully");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}



