using Microsoft.AspNetCore.Mvc;
using TheSeatLineApi.Common;
using TheSeatLineApi.MasterServices.DTOs;
using TheSeatLineApi.MasterServices.Repository;

namespace TheSeatLineApi.MasterServices.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowSeatCategoryController : ControllerBase
    {
        private readonly IShowSeatCategoryRepository repository;

        public ShowSeatCategoryController(IShowSeatCategoryRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("GetShowSeatCategory")]
        public async Task<Response<List<ShowSeatCategorySelectDTO>>> GetShowSeatCategory(int showId)
        {
            try
            {
                return Response<List<ShowSeatCategorySelectDTO>>
                    .Ok(await repository.SelectShowSeatCategory(showId));
            }
            catch (Exception ex)
            {
                return Response<List<ShowSeatCategorySelectDTO>>
                    .Fail(ex.Message);
            }
        }

        [HttpPost("InsertShowSeatCategory")]
        public async Task<Response<int>> InsertShowSeatCategory(ShowSeatCategoryInsertDTO dto)
        {
            try
            {
                return Response<int>
                    .Ok(await repository.InsertShowSeatCategory(dto));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }

        [HttpPut("UpdateShowSeatCategory")]
        public async Task<Response<int>> UpdateShowSeatCategory(ShowSeatCategoryInsertDTO dto)
        {
            try
            {
                return Response<int>
                    .Ok(await repository.UpdateShowSeatCategory(dto));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }

        [HttpDelete("DeleteShowSeatCategory")]
        public async Task<Response<int>> DeleteShowSeatCategory(int id)
        {
            try
            {
                return Response<int>
                    .Ok(await repository.DeleteShowSeatCategory(id));
            }
            catch (Exception ex)
            {
                return Response<int>
                    .Fail(ex.Message);
            }
        }
    }
}
