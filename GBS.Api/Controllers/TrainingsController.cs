using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class TrainingsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public IActionResult GetAllTrainings()
        {
            // Hardcoded trainings data
            var trainingsOUT = new List<object>
    {
        new { ID = 1, Name = "Leadership Training", Duration = "3 Days", Location = "New York", Trainer = "John Doe" },
        new { ID = 2, Name = "Technical Workshop", Duration = "1 Week", Location = "San Francisco", Trainer = "Jane Smith" },
        new { ID = 3, Name = "Project Management", Duration = "2 Days", Location = "Chicago", Trainer = "Emily Johnson" }
    };

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = trainingsOUT,
                type = "Training"
            });
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("before")]
        public IActionResult GetPastTrainings(DateTime scheduleTime)
        {
            // Hardcoded past training data
            var result = new List<object>
    {
        new
        {
            ID = 101,
            TrainingId = 1,
            TrainingDate = new DateTime(2024, 10, 15, 9, 0, 0),
            Topic = "Leadership Training - Team Building",
            Trainers = new[] { new { ID = 1, Name = "John Doe" } },
            IsMandatory = true,
            IsOpenForSubscription = false,
            IsCompleted = true,
            Location = "New York"
        },
        new
        {
            ID = 102,
            TrainingId = 2,
            TrainingDate = new DateTime(2024, 11, 5, 14, 0, 0),
            Topic = "Technical Workshop - Advanced Techniques",
            Trainers = new[] { new { ID = 2, Name = "Jane Smith" } },
            IsMandatory = false,
            IsOpenForSubscription = true,
            IsCompleted = false,
            Location = "San Francisco"
        },
        new
        {
            ID = 103,
            TrainingId = 3,
            TrainingDate = new DateTime(2024, 9, 20, 10, 30, 0),
            Topic = "Project Management - Agile Methodologies",
            Trainers = new[] { new { ID = 3, Name = "Emily Johnson" } },
            IsMandatory = false,
            IsOpenForSubscription = false,
            IsCompleted = true,
            Location = "Chicago"
        }
    };

            // Returning hardcoded data
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = result,
                type = "PastTraining"
            });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("self/conducted")]
        public IActionResult GetMyConductedTrainings(DateTime scheduleTime)
        {
            // Hardcoded data for trainings conducted by the logged-in employee
            var result = new List<object>
    {
        new
        {
            ID = 201,
            TrainingId = 10,
            TrainingDate = new DateTime(2024, 10, 10, 10, 0, 0),
            Topic = "Effective Communication - Presentation Skills",
            Objective = "To enhance communication skills in the workplace.",
            Trainers = new[] { new { ID = 101, Name = "John Doe" } },
            IsMandatory = true,
            IsOpenForSubscription = false,
            IsCompleted = true
        },
        new
        {
            ID = 202,
            TrainingId = 11,
            TrainingDate = new DateTime(2024, 11, 12, 15, 0, 0),
            Topic = "Problem Solving - Analytical Thinking",
            Objective = "To develop analytical skills for complex problem-solving.",
            Trainers = new[] { new { ID = 102, Name = "Jane Smith" } },
            IsMandatory = false,
            IsOpenForSubscription = true,
            IsCompleted = false
        },
        new
        {
            ID = 203,
            TrainingId = 12,
            TrainingDate = new DateTime(2024, 9, 18, 9, 30, 0),
            Topic = "Time Management - Productivity Boost",
            Objective = "To improve time management for better productivity.",
            Trainers = new[] { new { ID = 103, Name = "Emily Johnson" } },
            IsMandatory = false,
            IsOpenForSubscription = false,
            IsCompleted = true
        }
    };

            // Returning the hardcoded data
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = result,
                type = "MyConductedTraining"
            });
        }




    }
}
