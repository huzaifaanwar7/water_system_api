using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("Recent/{count:int?}/{commentCount:int?}")]
        public IActionResult GetRecentDiscussions(int count = 2, int commentCount = 2)
        {
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    discussions = new List<object>
            {
                new
                {
                    DiscussionID = 1,
                    title = "Discussion 1",
                    body = new List<object>
                    {
                        new { CommentID = 1, CommentText = "This is the first comment." },
                        new { CommentID = 2, CommentText = "This is the second comment." }
                    },
                    CommentCount = 2
                },
                new
                {
                    DiscussionID = 2,
                    title = "Discussion 2",
                    body = new List<object>
                    {
                        new { CommentID = 1, CommentText = "This is another comment." }
                    },
                    CommentCount = 1
                }
            },
                    message = "Recent Discussions"
                }
            });
        }
        [HttpGet]
        public IActionResult Get()
        {
            // Hardcoded response for demo purposes
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    Discussion = new List<object>
            {
                new
                {
                    ID = 1,
                    Title = "Discussion Topic 1",
                    Description = "This is the first discussion topic.",
                    CreatedDate = "2024-11-25",
                    CommentsCount = 5
                },
                new
                {
                    ID = 2,
                    Title = "Discussion Topic 2",
                    Description = "This is the second discussion topic.",
                    CreatedDate = "2024-11-24",
                    CommentsCount = 8
                }
            },
                    message = "Discussion"
                }
            });
        }
        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            // Hardcoded response for a specific discussion
            var discussion = new
            {
                ID = id,
                Title = $"Discussion Topic {id}",
                Description = $"This is the description for discussion topic {id}.",
                CreatedDate = "2024-11-25",
                CommentsCount = 10
            };

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = discussion,
                message = "Discussion"
            });
        }

        [HttpGet]
        [Route("Archive")]
        public IActionResult GetArchive()
        {
            // Hardcoded response for archived discussions
            var discussions = new List<object>
    {
        new
        {
            ID = 1,
            Title = "Archived Topic 1",
            Description = "This is the first archived discussion topic.",
            CreatedDate = "2023-11-25",
            CommentsCount = 5
        },
        new
        {
            ID = 2,
            Title = "Archived Topic 2",
            Description = "This is the second archived discussion topic.",
            CreatedDate = "2023-10-20",
            CommentsCount = 8
        }
    };

            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = discussions,
                message = "Discussion"
            });
        }




    }
}
