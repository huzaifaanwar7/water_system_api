using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class LOVController : ControllerBase
    {
        private static readonly List<dynamic> LOVs = new List<dynamic>
        {
            new { Id = 1, GroupName = "EmployeeStatus", Key = "Active", Name = "Active Employee", Description = "An active employee" },
            new { Id = 2, GroupName = "EmployeeStatus", Key = "Inactive", Name = "Inactive Employee", Description = "An inactive employee" },
            new { Id = 3, GroupName = "Department", Key = "HR", Name = "Human Resources", Description = "HR Department" },
            new { Id = 4, GroupName = "Department", Key = "IT", Name = "Information Technology", Description = "IT Department" },
        };

        /// <summary>
        /// Get all LOVs
        /// </summary>
        /// 
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllLOVs()
        {
            if (LOVs == null || !LOVs.Any())
            {
                return BadRequest(new { message = "Unable to load all LOVs. Try again later." });
            }

            return Ok(new { status = HttpStatusCode.OK, data = LOVs });
        }

        /// <summary>
        /// Get LOVs by group name
        /// </summary>
        [AllowAnonymous]
        [HttpGet("by-group")]
        public IActionResult GetLOVsByGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                return BadRequest(new { message = "Group name is required." });
            }

            var matchingLOVs = LOVs.Where(lov => lov.GroupName.ToLower() == groupName.ToLower()).ToList();

            if (!matchingLOVs.Any())
            {
                return BadRequest(new { message = $"No LOVs found for the group name '{groupName}'." });
            }

            return Ok(new { status = HttpStatusCode.OK, data = matchingLOVs });
        }

        /// <summary>
        /// Get LOVs by group name and key
        /// </summary>
        [AllowAnonymous]
        [HttpGet("by-group-and-key")]
        public IActionResult GetLOVsByGroupAndKey(string groupName, string key, bool isSystemField = false)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(key))
            {
                return BadRequest(new { message = "Group name and key are required." });
            }

            var matchingLOVs = LOVs.Where(lov =>
                lov.GroupName.ToLower() == groupName.ToLower() &&
                lov.Key.ToLower() == key.ToLower()).ToList();

            if (!matchingLOVs.Any())
            {
                return BadRequest(new { message = $"No LOVs found for group '{groupName}' and key '{key}'." });
            }

            return Ok(new { status = HttpStatusCode.OK, data = matchingLOVs });
        }
    }
}
