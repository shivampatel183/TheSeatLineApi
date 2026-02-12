using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static TheSeatLineApi.MasterServices.DTOs.EventDTOs;
using TheSeatLineApi.MasterServices.Repository;
using TheSeatLineApi.Common;

namespace TheSeatLineApi.MasterServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository eventRepository;

        public EventController(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        [HttpGet("GetEvent")]
        public async Task<Response<List<EventSelectDTO>>> GetEvent()
        {
            try
            {
                return Response<List<EventSelectDTO>>
                    .Ok(await eventRepository.SelectEvent());
            }
            catch (Exception ex)
            {
                return Response<List<EventSelectDTO>>
                    .Fail(ex.Message);
            }
        }

        [HttpGet("GetEventById")]
        public async Task<Response<EventSelectDTO>> GetEventById(int id)
        {
            try
            {
                return Response<EventSelectDTO>
                    .Ok(await eventRepository.SelectEventById(id));
            }
            catch
            {
                return Response<EventSelectDTO>
                    .Fail("Event Not Found");
            }
        }

        [HttpPost("InsertEvent")]
        public async Task<Response<int>> InsertEvent(EventInsertDTO dto)
        {
            try
            {
                return Response<int>
                    .Ok(await eventRepository.InsertEvent(dto));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }

        [HttpPut("UpdateEvent")]
        public async Task<Response<int>> UpdateEvent(EventInsertDTO dto)
        {
            try
            {
                return Response<int>
                    .Ok(await eventRepository.UpdateEvent(dto));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }

        [HttpDelete("DeleteEvent")]
        public async Task<Response<int>> DeleteEvent(int eventId)
        {
            try
            {
                return Response<int>
                    .Ok(await eventRepository.DeleteEvent(eventId));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }
    }
}
