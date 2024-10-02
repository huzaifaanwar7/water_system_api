using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PrescottAppBackend.Domain;
using PrescottAppBackend.Domain.DbModels;

namespace PrescottAppBackend.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _role;
        public RoleController(IRoleService role){
            _role = role;
        }
        
        [Route("create")]
        [HttpPost]
        // [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromBody] RoleVM roleVM)
        {
            try
            {
                var newRole = await _role.AddRoleAsync(roleVM);
                return Ok(newRole);
            }
            catch (Exception ex)
            {
                return BadRequest(new { nessage = ex.Message });
            }
        }
    }
}
