using System.Net;
using Microsoft.AspNetCore.Mvc;
using GlobularsAdminAppBackend.Api.Model;
using GlobularsAdminAppBackend.Domain;

namespace GlobularsAdminAppBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportedProblemController(IReportedProblemService _problemService) : ControllerBase
    {
        [HttpGet]
        public async Task<BaseResponse> Get()
        {
            try
            {
                var reportedProblem = await _problemService.GetAllReportedProblemsAsync();
                if (reportedProblem == null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        data = reportedProblem
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = reportedProblem
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

        [HttpGet("{id}")]
        public async Task<BaseResponse> Get(int id)
        {
            try
            {
                var reportedProblem = await _problemService.GetReportedProblemByIdAsync(id);
                if (reportedProblem == null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        data = reportedProblem
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = reportedProblem
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

        [HttpPost]
        public async Task<BaseResponse> AddUpdate([FromBody] ReportedProblemVM vM)
        {
            try
            {
                var reportedProblem = await _problemService.AddUpdateReportedProblemAsync(vM);
                return new BaseResponse()
                {
                    status = HttpStatusCode.OK,
                    data = reportedProblem,
                    message = "Data Saved",
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    data = ex,
                    message = ex.Message
                };
            }
        }


        [HttpDelete("{id}")]
        public async Task<BaseResponse> Delete(int id)
        {

            try
            {
                await _problemService.DeleteReportedProblemAsync(id);
                return new BaseResponse()
                {
                    status = HttpStatusCode.NoContent,
                    data = "",
                    message = "Deleted",
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse()
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

        [HttpGet("user-reported-problem/{userId}")]
        public async Task<BaseResponse> GetBuildingReportedProblem(string userId)
        {
            try
            {
                var reportedProblem = await _problemService.GetReportedProblemByUser(userId);
                if (reportedProblem == null)
                {
                    return new BaseResponse
                    {
                        status = HttpStatusCode.NotFound,
                        data = reportedProblem
                    };
                }
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = reportedProblem
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = ex.Message,
                    data = ex
                };
            }
        }

    }
}