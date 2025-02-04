using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class TrainingSkillsController : ControllerBase
    {

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public IActionResult GetAllTrainingSkills()
        {
            var skills = new List<object>
    {
        new { ID = 1, IsActive = true, Name = "Communication Skills" },
        new { ID = 2, IsActive = false, Name = "Leadership Skills" },
        new { ID = 3, IsActive = true, Name = "Time Management" }
    };

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = skills,
                
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            var skills = new List<object>
    {
        new { ID = 1, IsActive = true, Name = "Communication Skills" },
        new { ID = 2, IsActive = false, Name = "Leadership Skills" },
        new { ID = 3, IsActive = true, Name = "Time Management" }
    };

            var skill = skills.FirstOrDefault(s => (int)s.GetType().GetProperty("ID").GetValue(s) == id);

            if (skill == null)
            {
                return Ok(new
                {
                    status = HttpStatusCode.NotFound,
                    message = "Training Skill not found"
                });
            }

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = skill,
                
            });
        }






    }
}
