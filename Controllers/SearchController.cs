using GBS.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    // Global search ("More" module) - players, teams, tournaments, matches
    [AllowAnonymous]
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public SearchController(GBS_DbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> Global([FromQuery] string q, [FromQuery] int limit = 8)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new { players = Array.Empty<object>(), teams = Array.Empty<object>(), tournaments = Array.Empty<object>(), matches = Array.Empty<object>() });

            var players = await _db.Players
                .Where(p => !p.IsDeleted && p.ApprovalStatus == "Approved" &&
                            (p.FullName.Contains(q) || (p.City != null && p.City.Contains(q))))
                .Take(limit)
                .Select(p => new { p.Id, p.FullName, p.PhotoUrl, p.Role, p.TeamId, p.City })
                .ToListAsync();

            var teams = await _db.Teams
                .Where(t => !t.IsDeleted && t.ApprovalStatus == "Approved" &&
                            (t.Name.Contains(q) || t.ShortCode.Contains(q) || (t.City != null && t.City.Contains(q))))
                .Take(limit)
                .Select(t => new { t.Id, t.Name, t.ShortCode, t.LogoUrl, t.City, t.Category })
                .ToListAsync();

            var tournaments = await _db.Tournaments
                .Where(t => !t.IsDeleted && t.Name.Contains(q))
                .Take(limit)
                .Select(t => new { t.Id, t.Name, t.LogoUrl, t.Stage, t.MatchFormat, t.StartDate })
                .ToListAsync();

            var matches = await _db.Matches
                .Where(m => !m.IsDeleted && m.MatchName.Contains(q))
                .OrderByDescending(m => m.ScheduledStart)
                .Take(limit)
                .Select(m => new { m.Id, m.MatchName, m.ScheduledStart, m.MatchState, m.HomeTeamId, m.AwayTeamId })
                .ToListAsync();

            return Ok(new { players, teams, tournaments, matches });
        }
    }
}
