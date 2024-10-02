using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrescottAppBackend.Domain;

namespace PrescottAppBackend.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownListController : ControllerBase
    {
        private readonly IDDLService _ddlService;
        public DropdownListController(IDDLService ddlService)
        {
            _ddlService = ddlService;
        }

        // GET: api/dropdownlist/BusinessType
        [HttpGet("{type}")]
        public async Task<IActionResult> Get(string type)
        {
            var ddls = await _ddlService.GetDropdownListByTypeAsync(type);
            if (ddls == null)
            {
                return NotFound();
            }
            return Ok(ddls);
        }

    }
}
