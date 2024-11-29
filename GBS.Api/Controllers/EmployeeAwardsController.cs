using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeAwardsController : ControllerBase
    {
       

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
            FullName = "Shahzad Butt",
            PictureUrl = "http://localhost:49580/assets/images/shahzad.jpg",
            DesignationName = "Software Engineer",
            RecordCount = 1
        },
        new
        {
            ID = 2,
            FullName = "Muhammad Zeeshan",
            PictureUrl = "http://localhost:49580/assets/images/zee.jpg",
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
                type = "bravoleaderboard"
            });
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("BravoOfTheDay/{CardType}")]
        public IActionResult GetBravoCardByDate(string cardType)
        {
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    awards = new List<object>
            {
                new
                {
                    ID = 1,
                    FullName = "Shahzad Butt",
                    PictureUrl = "http://localhost:4200/assets/images/shahzad.jpg",
                    DesignationName = "Software Engineer",
                    RecordCount = 3
                },
                new
                {
                    ID = 2,
                    FullName = "Muhammad Zeeshan",
                    PictureUrl = "http://localhost:4200/assets/images/zee.jpg",
                    DesignationName = "Project Manager",
                    RecordCount = 3
                },
                new
                {
                    ID = 3,
                    FullName = "Alice Johnson",
                    PictureUrl = "https://example.com/images/alice_johnson.png",
                    DesignationName = "UX Designer",
                    RecordCount = 3
                }
            },
                    message = "BravoOfTheDay"
                }
            });
        }








    }
    
}
