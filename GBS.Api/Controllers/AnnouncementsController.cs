using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GBS.Api.Controller
{
    [Route("[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("top/{count}")]
        public IActionResult GetTopAnnouncements(int count)
        {
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    announcements = new List<object>
            {
                new
                {
                    id = 1,
                    title = "Top Announcement 1",
                    body = "This is the body of the first top announcement.",
                    Active = true,
                    publishOn = DateTime.Now,
                    SendEmail = true,
                    IsEmailSent = true,
                    IsBannerAnnouncement = false,
                    EventDate = DateTime.Now.AddDays(10)
                },
                new
                {
                    id = 2,
                    title = "Top Announcement 2",
                    body = "This is the body of the second top announcement.",
                    Active = true,
                    PublishOn = DateTime.Now.AddDays(-1),
                    SendEmail = false,
                    IsEmailSent = false,
                    IsBannerAnnouncement = true,
                    EventDate = DateTime.Now.AddDays(5)
                }
            },
                    message = "Top Announcements"
                }
            });

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public IActionResult GetAnnouncements()
        {
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    announcements = new List<object>
            {
                new
                {
                    id = 1,
                    title = "Top Announcement 1",
                    body = "This is the body of the first top announcement.",
                    Active = true,
                    publishOn = DateTime.Now,
                    SendEmail = true,
                    IsEmailSent = true,
                    IsBannerAnnouncement = false,
                    EventDate = DateTime.Now.AddDays(10)
                },
                new
                {
                    id = 2,
                    title = "Top Announcement 2",
                    body = "This is the body of the second top announcement.",
                    Active = true,
                    PublishOn = DateTime.Now.AddDays(-1),
                    SendEmail = false,
                    IsEmailSent = false,
                    IsBannerAnnouncement = true,
                    EventDate = DateTime.Now.AddDays(5)
                }
            },
                    message = "Top Announcements"
                }
            });

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("year/{year}")]
        public IActionResult GetAnnouncementsByYear(int year)
        {
            return Ok(new
            {
                status = HttpStatusCode.OK,
                data = new
                {
                    announcements = new List<object>
            {
                new
                {
                    id = 1,
                    title = $"Announcement for {year} - 1",
                    body = "This is the body of the first top announcement.",
                    Active = true,
                    publishOn = DateTime.Now,
                    SendEmail = true,
                    IsEmailSent = true,
                    IsBannerAnnouncement = false,
                    EventDate = DateTime.Now.AddDays(10)
                },
                new
                {
                    id = 2,
                    title = $"Announcement for {year} - 1",
                    body = $"This is the body of announcement for year {year} - 1.",
                    Active = true,
                    PublishOn = DateTime.Now.AddDays(-1),
                    SendEmail = false,
                    IsEmailSent = false,
                    IsBannerAnnouncement = true,
                    EventDate = DateTime.Now.AddDays(5)
                }
            },
                    message = "Top Announcements"
                }
            });

        }


    }
}
