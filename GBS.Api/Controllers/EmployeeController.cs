using Firebase.Auth;
using GBS.Api.Model;
using GBS.Data.Model;
using GBS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService _employeeService) : ControllerBase
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
       
        
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            int user = Convert.ToInt32(HttpContext.Items["EmployeeId"]);
            try
            {
                // Retrieve all users using the employee service
                var users = await _employeeService.GetEmployeeList();
                var baseUrls = "https://ihs.cc";
                // Check if users list is not empty
                if (users != null && users.Any())
                {
                    // Prepare the response data
                    var response = users.Select(user => new EmployeeVM
                    {
                        Id = user.Id,
                        Username= user.Username,
                        FullName = (user.FirstName + " " + user.LastName).Trim(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PersonalEmail = user.PersonalEmail,
                        PersonalPhone =  user.PersonalPhone,
                        //user.EmployeeTechStacks,
                        Cnic = user.Cnic,
                        //user.EmployeeJobRoles,
                        JoiningDate = user.JoiningDate, 
                        SeparationDate = user.SeparationDate,
                        ProfilePictureUrl = baseUrls + user.ProfilePictureUrl,

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

        
        [HttpGet("{EmployeeId}")]
        public async Task<IActionResult> GetDetails([FromRoute] int EmployeeId)
        {
            try
            {
                // Retrieve all users using the employee service
                var user = await _employeeService.GetEmployeeById(EmployeeId);
                var baseUrls = "https://ihs.cc";
                // Check if users list is not empty
                if (user != null)
                {
                    // Prepare the response data
                    var response = new EmployeeVM
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = (user.FirstName + " " + user.LastName).Trim(),
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PersonalEmail = user.PersonalEmail,
                        PersonalPhone = user.PersonalPhone,
                        //user.EmployeeTechStacks,
                        Cnic = user.Cnic,
                        //user.EmployeeJobRoles,
                        JoiningDate = user.JoiningDate,
                        SeparationDate = user.SeparationDate,
                        ProfilePictureUrl = baseUrls + user.ProfilePictureUrl,
                        JobRole = user.EmployeeJobRoles.Select(j => j.JobRoleIdFkNavigation.Name),
                        UserRole = user.EmployeeUserRoles.Select(j => j.UserRoleIdFkNavigation.Name),
                        TechStack = user.EmployeeTechStacks.Select(j => j.TeckStackIdFkNavigation.Name),
                        BankDetail = user.EmployeeBankDetails.Select(b => new BankDetailVM
                        {
                            Id = b.Id,
                            EmployeeId = b.EmployeeIdFk,
                            BankName = b.BankName,
                            AccountTitle = b.AccountTitle,
                            BranchCode = b.BranchCode,
                            AccountNumber = b.AccountNumber,
                            Iban = b.Iban
                        }),
                        Status = user.StatusIdFkNavigation?.Name,

                    };

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
