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
    [Route("api/matches/{matchId:int}")]
    public class ScoringController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public ScoringController(GBS_DbContext db) { _db = db; }

        public class StartInningsRequest
        {
            public int BattingTeamId { get; set; }
            public int BowlingTeamId { get; set; }
            public int InningsNumber { get; set; } = 1;
            public int? Target { get; set; }
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("innings")]
        public async Task<IActionResult> StartInnings(int matchId, [FromBody] StartInningsRequest req)
        {
            var match = await _db.Matches.FirstOrDefaultAsync(m => m.Id == matchId && !m.IsDeleted);
            if (match == null) return NotFound();

            var existing = await _db.Innings.FirstOrDefaultAsync(i => i.MatchId == matchId && i.InningsNumber == req.InningsNumber);
            if (existing != null) return Ok(existing);

            var innings = new Innings
            {
                MatchId = matchId,
                InningsNumber = req.InningsNumber,
                BattingTeamId = req.BattingTeamId,
                BowlingTeamId = req.BowlingTeamId,
                Target = req.Target
            };
            _db.Innings.Add(innings);
            if (match.MatchState == "Scheduled") { match.MatchState = "Live"; match.ActualStart = DateTime.UtcNow; }
            await _db.SaveChangesAsync();

            _db.Notifications.Add(new Notification {
                Type = "MatchStart",
                Title = $"Innings {req.InningsNumber} started",
                Body = match.MatchName,
                MatchId = matchId,
                TournamentId = match.TournamentId,
            });
            await _db.SaveChangesAsync();
            return Ok(innings);
        }

        public class RecordBallRequest
        {
            public Guid BallGuid { get; set; } = Guid.NewGuid();
            public int InningsId { get; set; }
            public int OverNumber { get; set; }
            public int BallInOver { get; set; }
            public int? StrikerPlayerId { get; set; }
            public int? NonStrikerPlayerId { get; set; }
            public int? BowlerPlayerId { get; set; }
            public int RunsBatter { get; set; } = 0;
            public int RunsExtras { get; set; } = 0;
            public string? ExtrasType { get; set; }
            public bool IsLegalDelivery { get; set; } = true;
            public bool IsFreeHit { get; set; } = false;
            public bool IsWicket { get; set; } = false;
            public string? WicketType { get; set; }
            public int? DismissedPlayerId { get; set; }
            public int? FielderPlayerId { get; set; }
            public string? Commentary { get; set; }
            public string? ClientDeviceId { get; set; }
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("balls")]
        public async Task<IActionResult> RecordBall(int matchId, [FromBody] RecordBallRequest req)
        {
            var dup = await _db.Balls.FirstOrDefaultAsync(b => b.BallGuid == req.BallGuid);
            if (dup != null) return Ok(dup);

            var innings = await _db.Innings.FirstOrDefaultAsync(i => i.Id == req.InningsId && i.MatchId == matchId);
            if (innings == null) return NotFound(new { message = "Innings not found." });

            var seq = await _db.Balls.CountAsync(b => b.InningsId == innings.Id && !b.IsUndone) + 1;

            var ball = new Ball
            {
                BallGuid = req.BallGuid,
                InningsId = innings.Id,
                MatchId = matchId,
                OverNumber = req.OverNumber,
                BallInOver = req.BallInOver,
                BallSequence = seq,
                StrikerPlayerId = req.StrikerPlayerId,
                NonStrikerPlayerId = req.NonStrikerPlayerId,
                BowlerPlayerId = req.BowlerPlayerId,
                RunsBatter = req.RunsBatter,
                RunsExtras = req.RunsExtras,
                ExtrasType = req.ExtrasType,
                IsLegalDelivery = req.IsLegalDelivery,
                IsFreeHit = req.IsFreeHit,
                IsWicket = req.IsWicket,
                WicketType = req.WicketType,
                DismissedPlayerId = req.DismissedPlayerId,
                FielderPlayerId = req.FielderPlayerId,
                Commentary = req.Commentary,
                ScoredByUserId = User.GetUserId(),
                BowledAt = DateTime.UtcNow
            };
            _db.Balls.Add(ball);

            // Apply to innings totals
            innings.TotalRuns += req.RunsBatter + req.RunsExtras;
            if (req.IsLegalDelivery) innings.LegalBallsBowled += 1;
            if (req.IsWicket) innings.Wickets += 1;
            switch (req.ExtrasType)
            {
                case "Wide":    innings.ExtrasWides   += req.RunsExtras; break;
                case "NoBall":  innings.ExtrasNoBalls += req.RunsExtras; break;
                case "Bye":     innings.ExtrasByes    += req.RunsExtras; break;
                case "LegBye":  innings.ExtrasLegByes += req.RunsExtras; break;
                case "Penalty": innings.ExtrasPenalty += req.RunsExtras; break;
            }

            // -------- BattingScore --------
            int batterRunsBefore = 0;
            if (req.StrikerPlayerId.HasValue)
            {
                var bs = await _db.BattingScores.FirstOrDefaultAsync(b => b.InningsId == innings.Id && b.PlayerId == req.StrikerPlayerId);
                if (bs == null)
                {
                    var nextOrder = await _db.BattingScores.CountAsync(b => b.InningsId == innings.Id) + 1;
                    bs = new BattingScore { InningsId = innings.Id, PlayerId = req.StrikerPlayerId.Value, BattingOrder = nextOrder };
                    _db.BattingScores.Add(bs);
                }
                batterRunsBefore = bs.Runs;
                // Batter only credited for runs not via Bye/LegBye/Wide
                if (req.ExtrasType is null or "NoBall")
                {
                    bs.Runs += req.RunsBatter;
                    if (req.RunsBatter == 4) bs.Fours++;
                    if (req.RunsBatter == 6) bs.Sixes++;
                }
                if (req.IsLegalDelivery) bs.BallsFaced++;
            }

            // Dismissed batter
            if (req.IsWicket && req.DismissedPlayerId.HasValue)
            {
                var dbs = await _db.BattingScores.FirstOrDefaultAsync(b => b.InningsId == innings.Id && b.PlayerId == req.DismissedPlayerId);
                if (dbs == null)
                {
                    var nextOrder = await _db.BattingScores.CountAsync(b => b.InningsId == innings.Id) + 1;
                    dbs = new BattingScore { InningsId = innings.Id, PlayerId = req.DismissedPlayerId.Value, BattingOrder = nextOrder };
                    _db.BattingScores.Add(dbs);
                }
                dbs.IsOut = true;
                dbs.DismissalDescription = string.IsNullOrEmpty(req.WicketType) ? "Out" : req.WicketType;
            }

            // -------- BowlingFigure --------
            if (req.BowlerPlayerId.HasValue)
            {
                var bf = await _db.BowlingFigures.FirstOrDefaultAsync(b => b.InningsId == innings.Id && b.PlayerId == req.BowlerPlayerId);
                if (bf == null)
                {
                    bf = new BowlingFigure { InningsId = innings.Id, PlayerId = req.BowlerPlayerId.Value };
                    _db.BowlingFigures.Add(bf);
                }
                if (req.IsLegalDelivery) bf.LegalBalls++;
                bf.RunsConceded += req.RunsBatter + req.RunsExtras;
                if (req.IsWicket && req.WicketType != "RunOut") bf.Wickets++;
                if (req.RunsBatter == 0 && (req.ExtrasType == null) && req.IsLegalDelivery) bf.Dots++;
                if (req.RunsBatter == 4) bf.Fours++;
                if (req.RunsBatter == 6) bf.Sixes++;
                if (req.ExtrasType == "Wide") bf.Wides++;
                if (req.ExtrasType == "NoBall") bf.NoBalls++;
            }

            await _db.SaveChangesAsync(); // ball saved → has Id

            // -------- Fall of wicket --------
            if (req.IsWicket && req.DismissedPlayerId.HasValue)
            {
                _db.FallOfWickets.Add(new FallOfWicket
                {
                    InningsId = innings.Id,
                    BallId = ball.Id,
                    WicketNumber = innings.Wickets,
                    Runs = innings.TotalRuns,
                    LegalBallsAt = innings.LegalBallsBowled,
                    DismissedPlayerId = req.DismissedPlayerId.Value,
                });
            }

            // -------- Commentary --------
            string? batterName = req.StrikerPlayerId.HasValue
                ? (await _db.Players.Where(p => p.Id == req.StrikerPlayerId).Select(p => p.FullName).FirstOrDefaultAsync()) : null;
            string? bowlerName = req.BowlerPlayerId.HasValue
                ? (await _db.Players.Where(p => p.Id == req.BowlerPlayerId).Select(p => p.FullName).FirstOrDefaultAsync()) : null;
            string? fielderName = req.FielderPlayerId.HasValue
                ? (await _db.Players.Where(p => p.Id == req.FielderPlayerId).Select(p => p.FullName).FirstOrDefaultAsync()) : null;

            var commentaryText = !string.IsNullOrEmpty(req.Commentary)
                ? req.Commentary
                : CommentaryFactory.Build(req.RunsBatter, req.RunsExtras, req.ExtrasType, req.IsWicket, req.WicketType, batterName, bowlerName, fielderName);

            var comm = new Commentary {
                BallId = ball.Id, MatchId = matchId,
                Text = commentaryText,
                IsOverridden = !string.IsNullOrEmpty(req.Commentary)
            };
            _db.Commentaries.Add(comm);

            // Milestone commentary
            if (req.StrikerPlayerId.HasValue && batterName != null)
            {
                var batterRunsAfter = batterRunsBefore + ((req.ExtrasType is null or "NoBall") ? req.RunsBatter : 0);
                var (mType, mLine) = CommentaryFactory.MilestoneFor(batterRunsBefore, batterRunsAfter, batterName);
                if (mType != null)
                {
                    _db.Commentaries.Add(new Commentary {
                        BallId = ball.Id, MatchId = matchId,
                        Text = mLine!, IsMilestone = true, MilestoneType = mType,
                    });
                    _db.Notifications.Add(new Notification {
                        Type = mType, Title = $"{batterName} reaches {(mType == "Fifty" ? "50" : "100")}",
                        Body = mLine, MatchId = matchId
                    });
                }
            }

            // Wicket notification
            if (req.IsWicket)
            {
                _db.Notifications.Add(new Notification {
                    Type = "Wicket",
                    Title = "Wicket!",
                    Body = commentaryText,
                    MatchId = matchId,
                });
            }

            // Outbox audit
            _db.OutboxBalls.Add(new OutboxBall {
                BallId = ball.Id, BallGuid = ball.BallGuid,
                MatchId = matchId, InningsId = innings.Id,
                ClientDeviceId = req.ClientDeviceId
            });

            await _db.SaveChangesAsync();
            return Ok(new { ball, innings, commentary = comm.Text });
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("balls/undo")]
        public async Task<IActionResult> UndoLastBall(int matchId)
        {
            var last = await _db.Balls
                .Where(b => b.MatchId == matchId && !b.IsUndone)
                .OrderByDescending(b => b.BallSequence)
                .FirstOrDefaultAsync();
            if (last == null) return NotFound(new { message = "Nothing to undo." });

            var innings = await _db.Innings.FirstOrDefaultAsync(i => i.Id == last.InningsId);
            if (innings != null)
            {
                innings.TotalRuns -= last.RunsBatter + last.RunsExtras;
                if (last.IsLegalDelivery) innings.LegalBallsBowled -= 1;
                if (last.IsWicket) innings.Wickets -= 1;
                switch (last.ExtrasType)
                {
                    case "Wide":    innings.ExtrasWides   -= last.RunsExtras; break;
                    case "NoBall":  innings.ExtrasNoBalls -= last.RunsExtras; break;
                    case "Bye":     innings.ExtrasByes    -= last.RunsExtras; break;
                    case "LegBye":  innings.ExtrasLegByes -= last.RunsExtras; break;
                    case "Penalty": innings.ExtrasPenalty -= last.RunsExtras; break;
                }
            }
            last.IsUndone = true;
            await _db.SaveChangesAsync();
            return Ok(new { ballId = last.Id, innings });
        }

        // Public — viewers poll this for live updates.
        [AllowAnonymous]
        [HttpGet("scorecard")]
        public async Task<IActionResult> Scorecard(int matchId)
        {
            var match = await _db.Matches.FirstOrDefaultAsync(m => m.Id == matchId && !m.IsDeleted);
            if (match == null) return NotFound();

            var innings = await _db.Innings.Where(i => i.MatchId == matchId)
                                            .OrderBy(i => i.InningsNumber).ToListAsync();
            var inningsIds = innings.Select(i => i.Id).ToList();

            var batting = await (from b in _db.BattingScores.Where(x => inningsIds.Contains(x.InningsId))
                                  join p in _db.Players on b.PlayerId equals p.Id
                                  select new {
                                      b.InningsId, b.PlayerId, p.FullName, b.BattingOrder,
                                      b.Runs, b.BallsFaced, b.Fours, b.Sixes,
                                      b.IsOut, b.DismissalDescription
                                  }).ToListAsync();
            var bowling = await (from f in _db.BowlingFigures.Where(x => inningsIds.Contains(x.InningsId))
                                  join p in _db.Players on f.PlayerId equals p.Id
                                  select new {
                                      f.InningsId, f.PlayerId, p.FullName,
                                      Overs = $"{f.LegalBalls / 6}.{f.LegalBalls % 6}",
                                      f.RunsConceded, f.Wickets, f.Dots, f.Fours, f.Sixes, f.Wides, f.NoBalls
                                  }).ToListAsync();
            var fow = await (from f in _db.FallOfWickets.Where(x => inningsIds.Contains(x.InningsId))
                              join p in _db.Players on f.DismissedPlayerId equals p.Id
                              orderby f.WicketNumber
                              select new {
                                  f.InningsId, f.WicketNumber, f.Runs,
                                  Overs = $"{f.LegalBallsAt / 6}.{f.LegalBallsAt % 6}",
                                  Player = p.FullName
                              }).ToListAsync();
            var recentBalls = await _db.Balls
                .Where(b => b.MatchId == matchId && !b.IsUndone)
                .OrderByDescending(b => b.BallSequence).Take(18)
                .Select(b => new {
                    b.Id, b.InningsId, b.OverNumber, b.BallInOver, b.RunsBatter, b.RunsExtras,
                    b.ExtrasType, b.IsWicket, b.WicketType, b.IsLegalDelivery,
                    b.IsFreeHit, b.Commentary, b.BowledAt, b.BallSequence
                }).ToListAsync();

            return Ok(new { match, innings, batting, bowling, fallOfWickets = fow, recentBalls });
        }

        [AllowAnonymous]
        [HttpGet("commentary")]
        public async Task<IActionResult> CommentaryFeed(int matchId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var q = _db.Commentaries.Where(c => c.MatchId == matchId).OrderByDescending(c => c.Id);
            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { items, page, pageSize, totalCount = total });
        }

        // WhatsApp-friendly text block. The image card / PDF endpoints are out of
        // scope without native rendering libs; this text-only endpoint covers the
        // share use-case for v1.
        [AllowAnonymous]
        [HttpGet("share")]
        public async Task<IActionResult> Share(int matchId)
        {
            var card = await Scorecard(matchId);
            if (card is not OkObjectResult ok || ok.Value is null) return NotFound();

            var match = await _db.Matches.FindAsync(matchId);
            var home = await _db.Teams.FindAsync(match!.HomeTeamId);
            var away = await _db.Teams.FindAsync(match.AwayTeamId);
            var innings = await _db.Innings.Where(i => i.MatchId == matchId).OrderBy(i => i.InningsNumber).ToListAsync();

            var lines = new List<string>
            {
                $"🏏 {match.MatchName}",
                $"{home?.Name} vs {away?.Name}",
                ""
            };
            foreach (var i in innings)
            {
                var batTeam = i.BattingTeamId == match.HomeTeamId ? home?.ShortCode : away?.ShortCode;
                var overs = $"{i.LegalBallsBowled / 6}.{i.LegalBallsBowled % 6}";
                lines.Add($"{batTeam}: {i.TotalRuns}/{i.Wickets} ({overs} ov)");
            }
            if (!string.IsNullOrEmpty(match.ResultMargin))
                lines.Add($"\nResult: {match.ResultMargin}");

            return Ok(new { text = string.Join('\n', lines) });
        }

        public class PenaltyRequest
        {
            public int? HomePenaltyRuns { get; set; }
            public int? AwayPenaltyRuns { get; set; }
            public string? PenaltyReason { get; set; }
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("penalty")]
        public async Task<IActionResult> ApplyPenalty(int matchId, [FromBody] PenaltyRequest req)
        {
            var match = await _db.Matches.FirstOrDefaultAsync(m => m.Id == matchId && !m.IsDeleted);
            if (match == null) return NotFound();
            if (req.HomePenaltyRuns.HasValue) match.HomePenaltyRuns = req.HomePenaltyRuns.Value;
            if (req.AwayPenaltyRuns.HasValue) match.AwayPenaltyRuns = req.AwayPenaltyRuns.Value;
            if (!string.IsNullOrWhiteSpace(req.PenaltyReason)) match.PenaltyReason = req.PenaltyReason;
            match.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(match);
        }

        // Close innings — used when target reached, all out, overs done, or abandoned.
        public class CloseInningsRequest { public string Reason { get; set; } = "OversComplete"; }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("innings/{inningsId:int}/close")]
        public async Task<IActionResult> CloseInnings(int matchId, int inningsId, [FromBody] CloseInningsRequest req)
        {
            var innings = await _db.Innings.FirstOrDefaultAsync(i => i.Id == inningsId && i.MatchId == matchId);
            if (innings == null) return NotFound();
            innings.IsClosed = true;
            innings.ClosedAt = DateTime.UtcNow;
            innings.ClosedReason = req.Reason;
            var match = await _db.Matches.FindAsync(matchId);
            if (match != null && innings.InningsNumber == 1) match.MatchState = "InningsBreak";
            await _db.SaveChangesAsync();
            return Ok(innings);
        }
    }
}
