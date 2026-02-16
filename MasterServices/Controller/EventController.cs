using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static TheSeatLineApi.MasterServices.DTOs.EventDTOs;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;
using TheSeatLineApi.Common;
using Microsoft.AspNetCore.Authorization;

namespace TheSeatLineApi.MasterServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        /// <summary>
        /// Get events in a specific city
        /// </summary>
        [HttpGet("city/{cityId}")]
        public async Task<Response<List<EventSelectDTO>>> GetEventsByCity(
            int cityId,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null)
        {
            try
            {
                var events = await eventRepository.GetEventsByCityAsync(cityId, fromDate, toDate);
                return Response<List<EventSelectDTO>>.Ok(
                    events,
                    $"Found {events.Count} event(s) in the selected city."
                );
            }
            catch (Exception ex)
            {
                return Response<List<EventSelectDTO>>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Discover nearby events based on location (GPS or City)
        /// </summary>
        [HttpGet("nearby")]
        public async Task<Response<List<EventSelectDTO>>> GetNearbyEvents([FromQuery] EventLocationQueryDto query)
        {
            try
            {
                if (!query.CityId.HasValue && (!query.Latitude.HasValue || !query.Longitude.HasValue))
                {
                    return Response<List<EventSelectDTO>>.Fail(
                        "Please provide either a CityId or both Latitude and Longitude."
                    );
                }

                var events = await eventRepository.GetNearbyEventsAsync(query);
                
                var message = query.CityId.HasValue
                    ? $"Found {events.Count} event(s) in the selected city."
                    : $"Found {events.Count} event(s) within {query.RadiusKm}km of your location.";

                return Response<List<EventSelectDTO>>.Ok(events, message);
            }
            catch (Exception ex)
            {
                return Response<List<EventSelectDTO>>.Fail(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
