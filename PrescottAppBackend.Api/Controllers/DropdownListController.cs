using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PrescottAppBackend.Api.Model;
using PrescottAppBackend.Domain;
using System.Net;

namespace PrescottAppBackend.Api;
[Route("api/[controller]")]
[ApiController]
public class DropdownListController(IDDLService _ddlService) : ControllerBase
{
    [HttpGet("{type}")]
    public async Task<BaseResponse> Get(string type)
    {
        try
        {
            var ddls = await _ddlService.GetDropdownListByTypeAsync(type);
            if (ddls == null)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.OK,
                    data = ddls
                };
            }
            return new BaseResponse
            {
                status = HttpStatusCode.OK,
                data = ddls
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


