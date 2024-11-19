using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MyApp.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("UpcomingCampaigns/{locallocation}")]
        public IActionResult GetUpcomingCampaign(string locallocation)
        {
            
            return Ok(new 
            {
                status = HttpStatusCode.OK,
                data = new 
                {
                    campaigns = new List<object>
                    {
                        new 
                        {
                            ID = 1,
                            Name = "Dummy Campaign 1",
                            LoginBannerImageWeb = "https://example.com/Documents/Campaign/dummy1_web.png",
                            LoginBannerImageMobile = "https://example.com/Documents/Campaign/dummy1_mobile.png",
                            DashboardBanner = new { /* Set necessary properties */ },
                            Description = "This is a dummy campaign for testing purposes."
                        },
                        new 
                        {
                            ID = 2,
                            Name = "Dummy Campaign 2",
                            LoginBannerImageWeb = "https://example.com/Documents/Campaign/dummy2_web.png",
                            LoginBannerImageMobile = "https://example.com/Documents/Campaign/dummy2_mobile.png",
                            DashboardBanner = new { /* Set necessary properties */ },
                            Description = "This is another dummy campaign for testing purposes."
                        }
                    },
                    message = "GetUpcomingCampaign"
                }
            });
        }
    }
}
