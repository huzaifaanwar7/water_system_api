using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class LunchesController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("Weekly")]
        public IActionResult GetLunchByWeek()
        {
            // Hardcoded response for demo purposes
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    LunchByWeek = new List<object>
            {
                new
                {
                    ID = 1,
                    MenuItem = "Grilled Chicken with Rice",
                    IsActive = true,
                    LunchDate = "2024-11-27",
                    LunchType = "Non-Vegetarian",
                    Location = "Head Office"
                },
                new
                {
                    ID = 2,
                    MenuItem = "Vegetable Pasta",
                    IsActive = true,
                    LunchDate = "2024-11-28",
                    LunchType = "Vegetarian",
                    Location = "Regional Office"
                }
            },
                    message = "LunchByWeek"
                }
            });
        }




    }
}
