using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Shared;
using TheSeatLineApi.Modules.MasterModule.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace TheSeatLineApi.Modules.MasterModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository _cityService;

        public CityController(ICityRepository cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<Response<List<CityListDto>>> GetAll()
        {
            try
            {
                return Response<List<CityListDto>>.Ok(await _cityService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return Response<List<CityListDto>>.Fail(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<CityDetailDto>> GetById(Guid id)
        {
            try
            {
                var city = await _cityService.GetByIdAsync(id);
                return city == null
                    ? Response<CityDetailDto>.Fail("City not found")
                    : Response<CityDetailDto>.Ok(city);
            }
            catch (Exception ex)
            {
                return Response<CityDetailDto>.Fail(ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Response<Guid>> Create(CityCreateDto dto)
        {
            try
            {
                return Response<Guid>.Ok(await _cityService.CreateAsync(dto));
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<Response<string>> Update(Guid id, CityUpdateDto dto)
        {
            try
            {
                await _cityService.UpdateAsync(id, dto);
                return Response<string>.Ok(null, "City updated");
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
                await _cityService.DeleteAsync(id);
                return Response<string>.Ok(null, "City deleted");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}



