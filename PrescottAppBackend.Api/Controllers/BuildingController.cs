using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PrescottAppBackend.Api.Model;
using PrescottAppBackend.Domain;
using System.Net;

namespace PrescottAppBackend.Api;

[Route("api/[controller]")]
[ApiController]
public class BuildingController(IBuildingService _buildingService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<BaseResponse> Get()
    {
        try
        {
            var buildings = await _buildingService.GetAllBuildingsAsync();
            if (buildings == null)
            {
                return new BaseResponse
                {
                    status = HttpStatusCode.NotFound,
                    data = buildings
                };
            }
            return new BaseResponse
            {
                status = HttpStatusCode.OK,
                data = buildings
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


