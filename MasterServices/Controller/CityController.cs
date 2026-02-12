using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Common;
using TheSeatLineApi.MasterServices.Business;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly ICityRepository cityRepository;
        public CityController(ICityRepository cityRepository)
        {
            this.cityRepository = cityRepository;
        }

        [HttpGet("GetCity")]
        public async Task<Response<List<CitySelectDTO>>> GetCity()
        {
            try
            {
                return Response<List<CitySelectDTO>>.Ok(await cityRepository.SelectCity());
            }
            catch (Exception ex)
            {
                return Response<List<CitySelectDTO>>.Fail(ex.Message);
            }
        }

        [HttpPost("InsertCity")]
        public async Task<Response<int>> InsertCity(CityInsertDTO city)
        {
            try
            {
                return Response<int>.Ok(await cityRepository.InsertCity(city));
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message);
            }
        }

        [HttpPut("UpdateCity")]
        public async Task<Response<int>> UpdateCity(CityInsertDTO city)
        {
            try
            {
                return Response<int>.Ok(await cityRepository.UpdateCity(city));
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message);
            }
        }

        [HttpDelete("DeleteCity")]
        public async Task<Response<int>> DeleteCity(int cityId)
        {
            try
            {
                return Response<int>.Ok(await cityRepository.DeleteCity(cityId));
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message);
            }
        }

        [HttpGet("GetCityByName")]
        public async Task<Response<CitySelectDTO>> GetCitByName(string Name)
        {
            try
            {
                return Response<CitySelectDTO>.Ok(await cityRepository.SelectCityByName(Name));
            }
            catch(Exception ex)
            {
                return Response<CitySelectDTO>.Fail("City Not Found");
            }
        }

        [HttpGet("GetCityById")]
        public async Task<Response<CitySelectDTO>> GetCitById(int Id)
        {
            try
            {
                return Response<CitySelectDTO>.Ok(await cityRepository.SelectCityById(Id));
            }
            catch (Exception ex)
            {
                return Response<CitySelectDTO>.Fail("City Not Found");
            }
        }
    }
}
