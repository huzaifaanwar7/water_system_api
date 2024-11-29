using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeAttendanceController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("empWfoAttendance")]
        public IActionResult GetEmployeeOfficeAttendance()
        {
            // Hardcoded response for demo purposes
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    EmployeeId = 12345,
                    AttendanceRecords = new List<object>
            {
                new
                {
                    Date = "2024-11-01",
                    Status = "Present",
                    CheckIn = "09:00 AM",
                    CheckOut = "06:00 PM"
                },
                new
                {
                    Date = "2024-11-02",
                    Status = "Absent",
                    CheckIn = (string)null,
                    CheckOut = (string)null
                }
            },
                    message = "Employee Office Attendance"
                }
            });
        }



    }
}
