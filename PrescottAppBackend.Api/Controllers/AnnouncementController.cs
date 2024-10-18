using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PrescottAppBackend.Api.Model;
using PrescottAppBackend.Domain;
using System.Net;

namespace PrescottAppBackend.Api;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementController(IAnnouncementService _announcementService) : ControllerBase
{
    [HttpGet]
    public async Task<BaseResponse> Get()
    {
        try
        {
            var Aanouncements = await _announcementService.GetAllAnnouncementsAsync();
            if (Aanouncements == null)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.NotFound,
                    data = Aanouncements
                };
            }
            return new BaseResponse
            {
                status = HttpStatusCode.OK,
                data = Aanouncements
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
            var anouncements = await _announcementService.GetAnnouncementByIdAsync(id);
            if (anouncements == null)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.NotFound,
                    data = anouncements
                };
            }
            return new BaseResponse
            {
                status = HttpStatusCode.OK,
                data = anouncements
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
    public async Task<BaseResponse> AddUpdate([FromBody] AnnouncementVM vM)
    {
        try
        {
            var anouncement = await _announcementService.AddUpdateAnnouncementAsync(vM);
            return new BaseResponse()
            {
                status = HttpStatusCode.OK,
                data = anouncement,
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

    [HttpPost("upload")]
    public async Task<BaseResponse> UploadFile([FromBody] AnnouncementVM vM)
    {
        try
        {
            var anouncement = vM; //await _announcementService.AddUpdateAnnouncementAsync(vM);
            return new BaseResponse()
            {
                status = HttpStatusCode.OK,
                // data = anouncement,
                message = "Data Recieved",
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
            await _announcementService.DeleteAnnouncementAsync(id);
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

}


