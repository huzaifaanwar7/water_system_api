using Microsoft.AspNetCore.Mvc;
using PrescottAppBackend.Api.Model;
using PrescottAppBackend.Domain;
using System.Net;

namespace PrescottAppBackend.Api;
[ApiController]
[Route("api/[controller]")]
public class RoleController(IRoleService _role) : ControllerBase
{

    [HttpPost("create")]
    public async Task<BaseResponse> CreateRole([FromBody] RoleVM roleVM)
    {
        try
        {
            var newRole = await _role.AddRoleAsync(roleVM);
            return new BaseResponse
            {
                status = HttpStatusCode.OK,
                data = newRole
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

    [HttpGet]
    public async Task<BaseResponse> GetAllRoles()
    {
        try
        {
            var roles = await _role.GetAllRolesAsync();
            return new BaseResponse() {
                status = HttpStatusCode.OK,
                data = roles,
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
}
