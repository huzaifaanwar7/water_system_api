using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("UpcomingNews/{fromDate:DateTime}/{toDate:DateTime}")]
        public async Task<IActionResult> GetUpcomingNews(DateTime fromDate, DateTime toDate)
        {
            // Here you can use hardcoded data similar to your previous example
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    upcomingNews = new List<object>
            {
                new
                {
                    ID = 1,
                    Name = "John Doe - New Joining",
                    EventType = "Joining",
                    EventDate = "2024-12-01",
                    Description = "John Doe will be joining the company on December 1st, 2024.",
                    AdditionalInfo = new
                    {
                        Position = "Software Engineer",
                        Department = "Engineering"
                    }
                },
                new
                {
                    ID = 2,
                    Name = "Jane Smith - Anniversary",
                    EventType = "Anniversary",
                    EventDate = "2024-12-05",
                    Description = "Jane Smith's 5th work anniversary with the company.",
                    AdditionalInfo = new
                    {
                        Position = "Project Manager",
                        Department = "Project Management"
                    }
                },
                new
                {
                    ID = 3,
                    Name = "Bob Brown - Birthday",
                    EventType = "Birthday",
                    EventDate = "2024-12-10",
                    Description = "Bob Brown will be celebrating his birthday on December 10th, 2024.",
                    AdditionalInfo = new
                    {
                        Department = "Marketing"
                    }
                }
            },
                    message = "GetUpcomingNews"
                }
            });
        }


    }
}
