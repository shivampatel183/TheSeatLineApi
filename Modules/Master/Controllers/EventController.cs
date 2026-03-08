using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Modules.AuthModule.Helpers;
using TheSeatLineApi.Shared;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using TheSeatLineApi.Modules.MasterModule.Repositories;

namespace TheSeatLineApi.Modules.MasterModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventService;

        public EventController(IEventRepository eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<Response<List<EventSelectDTO>>> GetAll([FromQuery] EventLocationQueryDto query)
        {
            try
            {
                return Response<List<EventSelectDTO>>.Ok(await _eventService.GetAllAsync(query));
            }
            catch (Exception ex)
            {
                return Response<List<EventSelectDTO>>.Fail(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<EventDetailDTO>> GetById(Guid id)
        {
            try
            {
                var ev = await _eventService.GetByIdAsync(id)
                    ?? throw new Exception("Event not found");
                return Response<EventDetailDTO>.Ok(ev);
            }
            catch (Exception ex)
            {
                return Response<EventDetailDTO>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Guid>> Create(EventInsertDTO dto)
        {
            try
            {
                Guid currentUserId = ClaimsExtensions.GetUserId(User);
                return Response<Guid>.Ok(await _eventService.CreateAsync(dto, currentUserId), "Event created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Update(Guid id, EventInsertDTO dto)
        {
            try
            {
                Guid currentUserId = ClaimsExtensions.GetUserId(User);
                await _eventService.UpdateAsync(id, dto, currentUserId);
                return Response<string>.Ok(null, "Event updated successfully");
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
                Guid currentUserId = ClaimsExtensions.GetUserId(User);
                await _eventService.DeleteAsync(id, currentUserId);
                return Response<string>.Ok(null, "Event deleted successfully");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}



