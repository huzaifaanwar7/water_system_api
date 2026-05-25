using GBS.Api.Data;
using GBS.Api.DbModels;
using GBS.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/matches")]
    public class MatchesController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public MatchesController(GBS_DbContext db) { _db = db; }

        public class MatchRequest
        {
            public int? TournamentId { get; set; }
            public string MatchName { get; set; } = "";
            public int HomeTeamId { get; set; }
            public int AwayTeamId { get; set; }
            public string? Venue { get; set; }
            public DateTime ScheduledStart { get; set; }
            public string MatchFormat { get; set; } = "T20";
            public int OversPerInnings { get; set; } = 20;
            public int BallsPerOver { get; set; } = 6;
            public int HomePenaltyRuns { get; set; } = 0;
            public int AwayPenaltyRuns { get; set; } = 0;
            public string? PenaltyReason { get; set; }
            public string? StageLabel { get; set; }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? state, [FromQuery] int? teamId, [FromQuery] int? tournamentId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var q = _db.Matches.Where(m => !m.IsDeleted);
            if (!string.IsNullOrWhiteSpace(state)) q = q.Where(m => m.MatchState == state);
            if (teamId.HasValue) q = q.Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId);
            if (tournamentId.HasValue) q = q.Where(m => m.TournamentId == tournamentId);

            var total = await q.CountAsync();
            var matches = await q.OrderByDescending(m => m.ScheduledStart).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();

            // Hydrate team info
            var teamIds = matches.SelectMany(m => new[] { m.HomeTeamId, m.AwayTeamId }).Distinct().ToList();
            var teams = await _db.Teams.Where(t => teamIds.Contains(t.Id)).Select(t => new { t.Id, t.Name, t.ShortCode, t.LogoUrl }).ToListAsync();
            var lookup = teams.ToDictionary(t => t.Id);

            var items = matches.Select(m => new
            {
                m.Id, m.MatchName, m.TournamentId, m.Venue, m.ScheduledStart, m.MatchFormat, m.OversPerInnings,
                m.MatchState, m.ResultWinnerTeamId, m.ResultMargin, m.StageLabel,
                HomeTeam = lookup.GetValueOrDefault(m.HomeTeamId),
                AwayTeam = lookup.GetValueOrDefault(m.AwayTeamId)
            });

            return Ok(new { items, page, pageSize, totalCount = total, totalPages = (int)Math.Ceiling(total / (double)pageSize) });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var m = await _db.Matches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (m == null) return NotFound();
            var home = await _db.Teams.FirstOrDefaultAsync(t => t.Id == m.HomeTeamId);
            var away = await _db.Teams.FirstOrDefaultAsync(t => t.Id == m.AwayTeamId);
            return Ok(new { match = m, homeTeam = home, awayTeam = away });
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MatchRequest req)
        {
            if (req.HomeTeamId == req.AwayTeamId) return BadRequest(new { message = "Home and away teams must differ." });
            var m = new Match
            {
                TournamentId = req.TournamentId,
                MatchName = req.MatchName,
                HomeTeamId = req.HomeTeamId,
                AwayTeamId = req.AwayTeamId,
                Venue = req.Venue,
                ScheduledStart = req.ScheduledStart,
                MatchFormat = req.MatchFormat,
                OversPerInnings = req.OversPerInnings,
                BallsPerOver = req.BallsPerOver <= 0 ? 6 : req.BallsPerOver,
                HomePenaltyRuns = req.HomePenaltyRuns,
                AwayPenaltyRuns = req.AwayPenaltyRuns,
                PenaltyReason = req.PenaltyReason,
                StageLabel = req.StageLabel,
                CreatedByUserId = User.GetUserId(),
                MatchState = "Scheduled"
            };
            _db.Matches.Add(m);
            await _db.SaveChangesAsync();
            return Ok(m);
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MatchRequest req)
        {
            var m = await _db.Matches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (m == null) return NotFound();
            m.MatchName = req.MatchName ?? m.MatchName;
            m.Venue = req.Venue ?? m.Venue;
            m.ScheduledStart = req.ScheduledStart != default ? req.ScheduledStart : m.ScheduledStart;
            m.MatchFormat = req.MatchFormat ?? m.MatchFormat;
            m.OversPerInnings = req.OversPerInnings;
            if (req.BallsPerOver > 0) m.BallsPerOver = req.BallsPerOver;
            m.HomePenaltyRuns = req.HomePenaltyRuns;
            m.AwayPenaltyRuns = req.AwayPenaltyRuns;
            m.PenaltyReason = req.PenaltyReason ?? m.PenaltyReason;
            m.StageLabel = req.StageLabel ?? m.StageLabel;
            m.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(m);
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Matches.FindAsync(id);
            if (m == null) return NotFound();
            m.IsDeleted = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public class TossRequest { public int TossWinnerTeamId { get; set; } public string TossDecision { get; set; } = "Bat"; }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("{id}/toss")]
        public async Task<IActionResult> Toss(int id, [FromBody] TossRequest req)
        {
            var m = await _db.Matches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (m == null) return NotFound();
            m.TossWinnerTeamId = req.TossWinnerTeamId;
            m.TossDecision = req.TossDecision;
            m.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(m);
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("{id}/start")]
        public async Task<IActionResult> Start(int id)
        {
            var m = await _db.Matches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (m == null) return NotFound();
            m.MatchState = "Live";
            m.ActualStart = DateTime.UtcNow;
            m.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(m);
        }

        public class EndRequest { public int? WinnerTeamId { get; set; } public string? ResultMargin { get; set; } public int? ManOfTheMatchPlayerId { get; set; } }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("{id}/end")]
        public async Task<IActionResult> End(int id, [FromBody] EndRequest req)
        {
            var m = await _db.Matches.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (m == null) return NotFound();
            m.MatchState = "Completed";
            m.ActualEnd = DateTime.UtcNow;
            m.ResultWinnerTeamId = req.WinnerTeamId;
            m.ResultMargin = req.ResultMargin;
            m.ManOfTheMatchPlayerId = req.ManOfTheMatchPlayerId;
            m.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(m);
        }
    }
}
