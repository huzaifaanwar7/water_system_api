using GBS.Api.Model;
using GBS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeesController(IEmployeeService _employeeService) : ControllerBase
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
        [AllowAnonymous]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                // Retrieve all users using the employee service
                var users = await _employeeService.ValidateUserAsync();
                var baseUrls = "https://ihs.cc";
                // Check if users list is not empty
                if (users != null && users.Any())
                {
                    // Prepare the response data
                    var response = users.Select(user => new
                    {
                        user.Id,
                        user.UserName,
                        user.FirstName,
                        user.LastName,
                        user.PersonalEmail,
                        user.PersonalPhone,
                        user.TechStack,
                        user.Cnic,
                        user.Role,
                        JoiningDate = user.JoiningDate.ToString("yyyy-MM-dd"), 
                        SeparationDate = user.SeparationDate?.ToString("yyyy-MM-dd"),
                        ProfilePictureUrl = baseUrls + user.ProfilePictureUrl,
                        displayName = (user.FirstName + " " + user.LastName).Trim()

                    }).ToList();

                    // Return success response
                    return Ok(new BaseResponse
                    {
                        status = HttpStatusCode.OK,
                        data = response
                    });
                }

                // Handle case where no users are found
                return NotFound(new BaseResponse
                {
                    status = HttpStatusCode.NotFound,
                    message = "No users found."
                });
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                return StatusCode(500, new BaseResponse
                {
                    status = HttpStatusCode.InternalServerError,
                    message = $"An error occurred: {ex.Message}"
                });
            }
        }



    }
}
