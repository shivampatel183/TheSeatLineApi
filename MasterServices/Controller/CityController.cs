using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Common;
using TheSeatLineApi.MasterServices.DTOs;

namespace TheSeatLineApi.MasterServices.Controller
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
        public async Task<Response<List<CitySelectDto>>> GetAll()
        {
            try
            {
                return Response<List<CitySelectDto>>.Ok(await _cityService.GetAllAsync());
            }
            catch (Exception ex)
            {
                return Response<List<CitySelectDto>>.Fail(ex.Message);
            }
        }

        [HttpPost]
        public async Task<Response<Guid>> Create(City model)
        {
            try
            {
                return Response<Guid>.Ok(await _cityService.CreateAsync(model), "City created successfully");
            }
            catch (Exception ex)
            {
                return Response<Guid>.Fail(ex.Message);
            }
        }

        [HttpPut]
        public async Task<Response<string>> Update(City model)
        {
            try
            {
                await _cityService.UpdateAsync(model);
                return Response<string>.Ok(null, "City updated successfully");
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message);
            }
        }
    }
}
