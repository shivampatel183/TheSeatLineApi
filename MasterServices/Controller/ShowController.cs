using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Common;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly IShowRepository showRepository;

        public ShowController(IShowRepository showRepository)
        {
            this.showRepository = showRepository;
        }

        [HttpGet("GetShow")]
        public async Task<Response<List<ShowSelectDTO>>> GetShow()
        {
            try
            {
                return Response<List<ShowSelectDTO>>.Ok(await showRepository.SelectShow());
            }
            catch (Exception ex)
            {
                return Response<List<ShowSelectDTO>>.Fail(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("InsertShow")]
        public async Task<Response<int>> InsertShow(ShowInsertDTO dto)
        {
            try
            {
                return Response<int>.Ok(await showRepository.InsertShow(dto));
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateShow")]
        public async Task<Response<int>> UpdateShow(ShowInsertDTO dto)
        {
            try
            {
                return Response<int>.Ok(await showRepository.UpdateShow(dto));
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteShow")]
        public async Task<Response<int>> DeleteShow(int showId)
        {
            try
            {
                return Response<int>.Ok(await showRepository.DeleteShow(showId));
            }
            catch (Exception ex)
            {
                return Response<int>.Fail(ex.Message);
            }
        }
    }
}
