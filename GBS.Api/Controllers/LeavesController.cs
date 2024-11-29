using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class LeavesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("summary")]
        public IActionResult GetEmployeeLeaveSummary(int employeeid, int month, int year)
        {
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    totalLeaves = 30,
                    totalAvailedLeaves = 10,
                    vacationLeaves = 5,
                    paidLeaves = 7,
                    unPaidLeaves = 3,
                    month = month,
                    year = year,
                    employeeDetails = new List<object>
            {
                new { EmployeeID = employeeid, EmployeeName = "John Doe" }
            }
                },
                message = "EmployeeLeaveSummary"
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("leavecategory")]
        
        public IActionResult GetEmployeeLeaveCategories()
        {
            // Hardcoded logged-in employee location
            var loggedInEmployeeLocation = "Head Office";

            // Hardcoded leave categories from employee leave service
            var employeeLeaveCategories = new List<dynamic>
    {
        new { LeaveCategoryValue = "Annual", Name = "Annual Leave" },
        new { LeaveCategoryValue = "Sick", Name = "Sick Leave" },
        new { LeaveCategoryValue = "Maternity", Name = "Maternity Leave" }
    };

            // Hardcoded leave categories from LOV service
            var lovLeaveCategory = new List<dynamic>
    {
        new { Value = "Annual", Text = "Annual Leave" },
        new { Value = "Sick", Text = "Sick Leave" },
        new { Value = "Casual", Text = "Casual Leave" }
    };

            // Filter categories based on employeeLeaveCategories
            var leaveCategories = lovLeaveCategory
                .Where(x => employeeLeaveCategories.Any(y => y.LeaveCategoryValue == x.Value))
                .ToList();

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = leaveCategories,
                message = "lov"
            });
        }



    }
}
