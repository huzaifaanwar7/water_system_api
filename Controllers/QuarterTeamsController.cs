using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class QuarterTeamsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("Dashboard/{year}/{quarter}")]
        public IActionResult GetQuarterTeamDashboard(int year, int quarter)
        {
            // Simulate hardcoded data for demo purposes
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    team = new
                    {
                        Year = year,
                        Quarter = quarter,
                        TeamName = "Demo Team",
                        Members = new List<object>
                {
                    new { Id = 1, Name = "Shahzad", Role = "Developer" },
                    new { Id = 2, Name = "Zeeshan", Role = "Tester" }
                },
                        Achievements = new List<string> { "Completed Project X", "Improved QA process" },
                        Challenges = new List<string> { "Resource constraints", "Tight deadlines" }
                    },
                    
                }
            });
        }


    }
}
