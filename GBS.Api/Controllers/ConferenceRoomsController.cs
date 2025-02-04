using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class ConferenceRoomsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetConferenceRooms()
        {
            // Hardcoded conference room data
            var conferenceRooms = new List<object>
            {
                new { ID = 1, Name = "Room A", Capacity = 10 },
                new { ID = 2, Name = "Room B", Capacity = 20 },
                new { ID = 3, Name = "Room C", Capacity = 15 },
            };

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = conferenceRooms
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Dashboard/{CardType}")]
        public IActionResult GetByMonthYearAndCardTypeDashboard(string cardType)
        {
            // Hardcoded employee awards data
            var employeeAwardsOutput = new List<object>
    {
        new
        {
            ID = 1,
            FullName = "John Doe",
            PictureUrl = "https://example.com/images/johndoe.jpg",
            DesignationName = "Software Engineer",
            RecordCount = 1
        },
        new
        {
            ID = 2,
            FullName = "Jane Smith",
            PictureUrl = "https://example.com/images/janesmith.jpg",
            DesignationName = "Product Manager",
            RecordCount = 2
        }
    };

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = employeeAwardsOutput
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("EmployeesKudosPoints/{count}")]
        public IActionResult GetEmployeesKudosPoints(int count)
        {
            // Hardcoded leaderboard data
            var leaderboardData = new List<object>
    {
        new
        {
            ID = 1,
            FullName = "John Doe",
            KudosPoints = 150,
            Rank = 1
        },
        new
        {
            ID = 2,
            FullName = "Jane Smith",
            KudosPoints = 120,
            Rank = 2
        },
        new
        {
            ID = 3,
            FullName = "Sam Brown",
            KudosPoints = 100,
            Rank = 3
        }
    };

            // Limit the leaderboard data to the specified count
            var limitedData = leaderboardData.Take(count).ToList();

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = limitedData,
                
            });
        }




    }

}
