using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Shared;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;

namespace TheSeatLineApi.Modules.MasterModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventShowController : ControllerBase
    {
        private readonly IEventShowRepository _eventShowService;

        public EventShowController(IEventShowRepository eventShowService)
        {
            _eventShowService = eventShowService;
        }

        [HttpGet]
        public async Task<Response<List<EventShowSelectDTO>>> GetAll([FromQuery] Guid? eventId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var shows = await _eventShowService.GetAllAsync(eventId, page, pageSize);
                return Response<List<EventShowSelectDTO>>.Ok(shows);
            }
            catch (Exception ex)
            {
                return Response<List<EventShowSelectDTO>>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Guid>> Create(EventShowInsertDTO dto)
        {
            try
            {
                return Response<Guid>.Ok(await _eventShowService.CreateAsync(dto), "Event show created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpGet("event/{eventId}")]
        public async Task<Response<List<EventShowSelectDTO>>> GetByEventId(Guid eventId)
        {
            try
            {
                var shows = await _eventShowService.GetByEventIdAsync(eventId);
                return Response<List<EventShowSelectDTO>>.Ok(shows);
            }
            catch (Exception ex)
            {
                return Response<List<EventShowSelectDTO>>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Update(Guid id, EventShowUpdateDTO dto)
        {
            try
            {
                await _eventShowService.UpdateAsync(id, dto);
                return Response<string>.Ok(null, "Event show updated successfully");
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
                await _eventShowService.DeleteAsync(id);
                return Response<string>.Ok(null, "Event show deleted successfully");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}



