using GBS.Api.Data;
using GBS.Api.DbModels;
using GBS.Api.Helpers;
using GBS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/players")]
    public class PlayersController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public PlayersController(GBS_DbContext db) { _db = db; }

        public class PlayerRequest
        {
            public string FullName { get; set; } = "";
            public DateTime? DateOfBirth { get; set; }
            public string? Role { get; set; }
            public string? BattingHandedness { get; set; }
            public string? BowlingStyle { get; set; }
            public string? City { get; set; }
            public int? TeamId { get; set; }
            public int? JerseyNumber { get; set; }
            public string? PhotoBase64 { get; set; }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] string? search,
            [FromQuery] int? teamId,
            [FromQuery] string? role,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var q = _db.Players.Where(p => !p.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(p => p.FullName.Contains(search) || (p.City != null && p.City.Contains(search)));
            if (teamId.HasValue) q = q.Where(p => p.TeamId == teamId);
            if (!string.IsNullOrWhiteSpace(role)) q = q.Where(p => p.Role == role);
            if (!string.IsNullOrWhiteSpace(status)) q = q.Where(p => p.ApprovalStatus == status);

            if (User.GetRole() != "SuperAdmin" && string.IsNullOrWhiteSpace(status))
                q = q.Where(p => p.ApprovalStatus == "Approved");

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(p => p.CreatedAt)
                               .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { items, page, pageSize, totalCount = total, totalPages = (int)Math.Ceiling(total / (double)pageSize) });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var player = await _db.Players.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (player == null) return NotFound();
            string? teamName = null;
            if (player.TeamId.HasValue)
                teamName = (await _db.Teams.FirstOrDefaultAsync(t => t.Id == player.TeamId))?.Name;
            return Ok(new { player, teamName });
        }

        [Authorize(Roles = "SuperAdmin,Captain")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlayerRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.FullName))
                return BadRequest(new { message = "FullName required." });

            var role = User.GetRole();
            var userId = User.GetUserId();

            // Captain can only add players to own team
            if (role == "Captain")
            {
                if (!req.TeamId.HasValue) return BadRequest(new { message = "Captain must specify TeamId." });
                var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == req.TeamId && t.CaptainUserId == userId);
                if (team == null) return Forbid();
            }

            var player = new Player
            {
                FullName = req.FullName.Trim(),
                DateOfBirth = req.DateOfBirth,
                Role = req.Role,
                BattingHandedness = req.BattingHandedness,
                BowlingStyle = req.BowlingStyle,
                City = req.City,
                TeamId = req.TeamId,
                JerseyNumber = req.JerseyNumber,
                CreatedByUserId = userId,
                ApprovalStatus = role == "SuperAdmin" ? "Approved" : "Pending",
                ApprovedByUserId = role == "SuperAdmin" ? userId : null,
                ApprovedAt = role == "SuperAdmin" ? DateTime.UtcNow : null
            };

            if (!string.IsNullOrEmpty(req.PhotoBase64))
            {
                var ext = req.PhotoBase64.Contains("image/png") ? ".png" : ".jpg";
                player.PhotoUrl = IOHelper.SaveFile(req.PhotoBase64, $"player_{Guid.NewGuid():N}{ext}", "players");
            }

            _db.Players.Add(player);
            await _db.SaveChangesAsync();

            if (req.TeamId.HasValue)
            {
                _db.TeamPlayers.Add(new TeamPlayer
                {
                    TeamId = req.TeamId.Value, PlayerId = player.Id, JerseyNumber = req.JerseyNumber, IsActive = true
                });
            }

            if (player.ApprovalStatus == "Pending")
            {
                _db.ApprovalRequests.Add(new ApprovalRequest
                {
                    EntityType = "Player",
                    EntityId = player.Id,
                    RequestedByUserId = userId ?? 0,
                    Status = "Pending",
                    Notes = $"New player: {player.FullName}"
                });
            }
            await _db.SaveChangesAsync();
            return Ok(player);
        }

        [Authorize(Roles = "SuperAdmin,Captain")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlayerRequest req)
        {
            var p = await _db.Players.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (p == null) return NotFound();

            var role = User.GetRole();
            var userId = User.GetUserId();
            if (role == "Captain")
            {
                if (!p.TeamId.HasValue) return Forbid();
                var owns = await _db.Teams.AnyAsync(t => t.Id == p.TeamId && t.CaptainUserId == userId);
                if (!owns) return Forbid();
            }

            p.FullName = req.FullName ?? p.FullName;
            p.DateOfBirth = req.DateOfBirth ?? p.DateOfBirth;
            p.Role = req.Role ?? p.Role;
            p.BattingHandedness = req.BattingHandedness ?? p.BattingHandedness;
            p.BowlingStyle = req.BowlingStyle ?? p.BowlingStyle;
            p.City = req.City ?? p.City;
            p.JerseyNumber = req.JerseyNumber ?? p.JerseyNumber;
            p.UpdatedAt = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(req.PhotoBase64))
            {
                var ext = req.PhotoBase64.Contains("image/png") ? ".png" : ".jpg";
                p.PhotoUrl = IOHelper.SaveFile(req.PhotoBase64, $"player_{Guid.NewGuid():N}{ext}", "players");
            }

            await _db.SaveChangesAsync();
            return Ok(p);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Players.FindAsync(id);
            if (p == null) return NotFound();
            p.IsDeleted = true;
            p.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Aggregate career stats from BattingScores + BowlingFigures across all matches.
        [AllowAnonymous]
        [HttpGet("{id}/stats")]
        public async Task<IActionResult> CareerStats(int id, [FromQuery] string? format, [FromQuery] int? tournamentId)
        {
            var player = await _db.Players.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (player == null) return NotFound();

            // Restrict by format/tournament via match join
            var inningsQ = from i in _db.Innings
                           join m in _db.Matches on i.MatchId equals m.Id
                           where !m.IsDeleted
                           select new { Innings = i, Match = m };
            if (!string.IsNullOrWhiteSpace(format)) inningsQ = inningsQ.Where(x => x.Match.MatchFormat == format);
            if (tournamentId.HasValue) inningsQ = inningsQ.Where(x => x.Match.TournamentId == tournamentId);
            var inningsIds = await inningsQ.Select(x => x.Innings.Id).ToListAsync();

            var bat = await _db.BattingScores
                .Where(b => inningsIds.Contains(b.InningsId) && b.PlayerId == id)
                .ToListAsync();
            var bowl = await _db.BowlingFigures
                .Where(b => inningsIds.Contains(b.InningsId) && b.PlayerId == id)
                .ToListAsync();

            // Batting aggregates
            var inningsBatted = bat.Count;
            var notOuts = bat.Count(b => !b.IsOut && b.BallsFaced > 0);
            var runs = bat.Sum(b => b.Runs);
            var ballsFaced = bat.Sum(b => b.BallsFaced);
            var fours = bat.Sum(b => b.Fours);
            var sixes = bat.Sum(b => b.Sixes);
            var highest = bat.Count == 0 ? 0 : bat.Max(b => b.Runs);
            var fifties = bat.Count(b => b.Runs >= 50 && b.Runs < 100);
            var hundreds = bat.Count(b => b.Runs >= 100);
            var outs = inningsBatted - notOuts;
            var average = outs == 0 ? (double?)null : Math.Round(runs / (double)outs, 2);
            var strikeRate = ballsFaced == 0 ? 0.0 : Math.Round((runs / (double)ballsFaced) * 100.0, 2);

            // Bowling aggregates
            var legalBalls = bowl.Sum(b => b.LegalBalls);
            var oversBowled = legalBalls / 6 + (legalBalls % 6) / 10.0;
            var runsConceded = bowl.Sum(b => b.RunsConceded);
            var wickets = bowl.Sum(b => b.Wickets);
            var maidens = bowl.Sum(b => b.Maidens);
            var dots = bowl.Sum(b => b.Dots);
            var bestWickets = bowl.Count == 0 ? 0 : bowl.Max(b => b.Wickets);
            var bestFigure = bowl.Where(b => b.Wickets == bestWickets)
                                  .OrderBy(b => b.RunsConceded)
                                  .FirstOrDefault();
            var bowlAvg = wickets == 0 ? (double?)null : Math.Round(runsConceded / (double)wickets, 2);
            var economy = legalBalls == 0 ? 0.0 : Math.Round(runsConceded / (legalBalls / 6.0), 2);
            var bowlSr  = wickets == 0 ? (double?)null : Math.Round(legalBalls / (double)wickets, 2);
            var fiveFers = bowl.Count(b => b.Wickets >= 5);

            // Matches played — distinct matches across batting + bowling lines
            var matchIdsB = await (from b in _db.BattingScores
                                    join i in _db.Innings on b.InningsId equals i.Id
                                    where b.PlayerId == id && inningsIds.Contains(i.Id)
                                    select i.MatchId).Distinct().ToListAsync();
            var matchIdsBow = await (from b in _db.BowlingFigures
                                     join i in _db.Innings on b.InningsId equals i.Id
                                     where b.PlayerId == id && inningsIds.Contains(i.Id)
                                     select i.MatchId).Distinct().ToListAsync();
            var matchesPlayed = matchIdsB.Union(matchIdsBow).Distinct().Count();

            // Last-N runs curve (default 10)
            var recent = await (from b in _db.BattingScores
                                join i in _db.Innings on b.InningsId equals i.Id
                                join m in _db.Matches on i.MatchId equals m.Id
                                where b.PlayerId == id && inningsIds.Contains(i.Id)
                                orderby m.ScheduledStart descending
                                select new { m.Id, m.MatchName, m.ScheduledStart, b.Runs, b.BallsFaced, b.IsOut }
                               ).Take(10).ToListAsync();

            return Ok(new
            {
                player.Id,
                player.FullName,
                player.Role,
                player.PhotoUrl,
                matchesPlayed,
                batting = new {
                    innings = inningsBatted, notOuts, runs, ballsFaced,
                    highestScore = highest, fours, sixes, fifties, hundreds,
                    average, strikeRate
                },
                bowling = new {
                    oversBowled, maidens, runsConceded, wickets, dots,
                    bestBowling = $"{bestWickets}/{bestFigure?.RunsConceded ?? 0}",
                    average = bowlAvg, economy, strikeRate = bowlSr, fiveWicketHauls = fiveFers
                },
                recentMatches = recent
            });
        }

        // Dedicated quick search endpoint (typeahead-friendly)
        [AllowAnonymous]
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q, [FromQuery] int limit = 10)
        {
            if (string.IsNullOrWhiteSpace(q)) return Ok(Array.Empty<object>());
            var results = await _db.Players
                .Where(p => !p.IsDeleted && p.ApprovalStatus == "Approved"
                            && (p.FullName.Contains(q) || (p.City != null && p.City.Contains(q))))
                .OrderBy(p => p.FullName)
                .Take(limit)
                .Select(p => new { p.Id, p.FullName, p.Role, p.PhotoUrl, p.TeamId, p.City })
                .ToListAsync();
            return Ok(results);
        }
    }
}
