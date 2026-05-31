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
    [Route("api/tournaments")]
    public class TournamentsController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public TournamentsController(GBS_DbContext db) { _db = db; }

        public class TournamentRequest
        {
            public string Name { get; set; } = "";
            public string? Edition { get; set; }
            public string? Category { get; set; }
            public string? Format { get; set; }
            public string? MatchFormat { get; set; }
            public int? OversPerInnings { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string? LogoBase64 { get; set; }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? search, [FromQuery] string? stage, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var q = _db.Tournaments.Where(t => !t.IsDeleted);
            if (!string.IsNullOrWhiteSpace(search)) q = q.Where(t => t.Name.Contains(search));
            if (!string.IsNullOrWhiteSpace(stage)) q = q.Where(t => t.Stage == stage);
            var total = await q.CountAsync();
            var items = await q.OrderByDescending(t => t.StartDate)
                               .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { items, page, pageSize, totalCount = total, totalPages = (int)Math.Ceiling(total / (double)pageSize) });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var t = await _db.Tournaments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (t == null) return NotFound();
            var teams = await (from tt in _db.TournamentTeams
                               join team in _db.Teams on tt.TeamId equals team.Id
                               where tt.TournamentId == id && !team.IsDeleted
                               select new { team.Id, team.Name, team.ShortCode, team.LogoUrl, tt.GroupName }).ToListAsync();
            var matches = await _db.Matches.Where(m => m.TournamentId == id && !m.IsDeleted).ToListAsync();
            return Ok(new { tournament = t, teams, matches });
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TournamentRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { message = "Name required." });

            var t = new Tournament
            {
                Name = req.Name.Trim(),
                Edition = req.Edition,
                Category = req.Category,
                Format = req.Format,
                MatchFormat = req.MatchFormat,
                OversPerInnings = req.OversPerInnings,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                CreatedByUserId = User.GetUserId()
            };
            if (!string.IsNullOrEmpty(req.LogoBase64))
            {
                var ext = req.LogoBase64.Contains("image/png") ? ".png" : ".jpg";
                t.LogoUrl = IOHelper.SaveFile(req.LogoBase64, $"tournament_{Guid.NewGuid():N}{ext}", "tournaments");
            }
            _db.Tournaments.Add(t);
            await _db.SaveChangesAsync();
            return Ok(t);
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TournamentRequest req)
        {
            var t = await _db.Tournaments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (t == null) return NotFound();
            t.Name = req.Name ?? t.Name;
            t.Edition = req.Edition ?? t.Edition;
            t.Category = req.Category ?? t.Category;
            t.Format = req.Format ?? t.Format;
            t.MatchFormat = req.MatchFormat ?? t.MatchFormat;
            t.OversPerInnings = req.OversPerInnings ?? t.OversPerInnings;
            t.StartDate = req.StartDate ?? t.StartDate;
            t.EndDate = req.EndDate ?? t.EndDate;
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(t);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var t = await _db.Tournaments.FindAsync(id);
            if (t == null) return NotFound();
            t.IsDeleted = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public class AddTeamRequest { public int TeamId { get; set; } public string? GroupName { get; set; } }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPost("{id}/teams")]
        public async Task<IActionResult> AddTeam(int id, [FromBody] AddTeamRequest req)
        {
            var t = await _db.Tournaments.FindAsync(id);
            if (t == null || t.IsDeleted) return NotFound();
            if (await _db.TournamentTeams.AnyAsync(tt => tt.TournamentId == id && tt.TeamId == req.TeamId))
                return Conflict(new { message = "Team already in tournament." });
            _db.TournamentTeams.Add(new TournamentTeam { TournamentId = id, TeamId = req.TeamId, GroupName = req.GroupName });
            await _db.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpDelete("{id}/teams/{teamId}")]
        public async Task<IActionResult> RemoveTeam(int id, int teamId)
        {
            var tt = await _db.TournamentTeams.FirstOrDefaultAsync(x => x.TournamentId == id && x.TeamId == teamId);
            if (tt == null) return NotFound();
            _db.TournamentTeams.Remove(tt);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        public class SetGroupRequest { public string? GroupName { get; set; } }

        // Assign / re-assign a team's pool (group) within the tournament.
        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPut("{id}/teams/{teamId}/group")]
        public async Task<IActionResult> SetTeamGroup(int id, int teamId, [FromBody] SetGroupRequest req)
        {
            var tt = await _db.TournamentTeams.FirstOrDefaultAsync(x => x.TournamentId == id && x.TeamId == teamId);
            if (tt == null) return NotFound();
            tt.GroupName = req.GroupName;
            await _db.SaveChangesAsync();
            return Ok();
        }

        // Computed live from completed matches + innings totals.
        // No materialized Standings table — derives W/L/T/NR + points + NRR + last-5 form.
        // Internal standings row used by both /standings and /qualifiers.
        private class StandingRow
        {
            public int TeamId { get; set; }
            public string? GroupName { get; set; }
            public object? Team { get; set; }
            public int MatchesPlayed { get; set; }
            public int Wins { get; set; }
            public int Losses { get; set; }
            public int Ties { get; set; }
            public int NoResults { get; set; }
            public int Points { get; set; }
            public double? NetRunRate { get; set; }
            public int RunsScored { get; set; }
            public double OversFaced { get; set; }
            public int RunsConceded { get; set; }
            public double OversBowled { get; set; }
            public List<string> Last5Form { get; set; } = new();
        }

        // Computes per-team standings (with pool/group) for a tournament.
        private async Task<List<StandingRow>> ComputeStandings(int id)
        {
            var tts = await _db.TournamentTeams
                .Where(tt => tt.TournamentId == id)
                .ToListAsync();
            var teamIds = tts.Select(tt => tt.TeamId).ToList();
            var groupOf = tts.ToDictionary(tt => tt.TeamId, tt => tt.GroupName);
            if (teamIds.Count == 0) return new List<StandingRow>();

            var matches = await _db.Matches
                .Where(m => m.TournamentId == id && !m.IsDeleted)
                .ToListAsync();
            var matchIds = matches.Select(m => m.Id).ToList();
            var innings = await _db.Innings
                .Where(i => matchIds.Contains(i.MatchId))
                .ToListAsync();
            var teams = await _db.Teams.Where(t => teamIds.Contains(t.Id))
                .Select(t => new { t.Id, t.Name, t.ShortCode, t.LogoUrl }).ToListAsync();
            var teamMap = teams.ToDictionary(t => t.Id);

            var rows = new List<StandingRow>();
            foreach (var teamId in teamIds)
            {
                var played = 0; var won = 0; var lost = 0; var tied = 0; var nr = 0;
                double runsScored = 0; double oversFaced = 0;
                double runsConceded = 0; double oversBowled = 0;
                var form = new List<string>(); // W/L/T/NR newest-first

                var teamMatches = matches.Where(m =>
                    (m.HomeTeamId == teamId || m.AwayTeamId == teamId) &&
                    (m.MatchState == "Completed" || m.MatchState == "Abandoned"))
                    .OrderByDescending(m => m.ScheduledStart).ToList();

                foreach (var m in teamMatches)
                {
                    played++;
                    string outcome;
                    if (m.MatchState == "Abandoned") { nr++; outcome = "NR"; }
                    else if (m.ResultWinnerTeamId == null)
                    {
                        // Tie when no winner but match completed.
                        tied++; outcome = "T";
                    }
                    else if (m.ResultWinnerTeamId == teamId) { won++; outcome = "W"; }
                    else { lost++; outcome = "L"; }
                    if (form.Count < 5) form.Add(outcome);

                    var batting = innings.Where(i => i.MatchId == m.Id && i.BattingTeamId == teamId).ToList();
                    var bowling = innings.Where(i => i.MatchId == m.Id && i.BowlingTeamId == teamId).ToList();
                    runsScored   += batting.Sum(i => i.TotalRuns);
                    oversFaced   += batting.Sum(i => i.LegalBallsBowled) / 6.0;
                    runsConceded += bowling.Sum(i => i.TotalRuns);
                    oversBowled  += bowling.Sum(i => i.LegalBallsBowled) / 6.0;
                }
                // Add penalty runs (against this team) to the opposition tally — keeps NRR honest.
                // Match.HomePenaltyRuns is awarded TO the opposition when HOME is penalized.
                foreach (var m in teamMatches)
                {
                    if (m.HomeTeamId == teamId && m.HomePenaltyRuns > 0) runsConceded += m.HomePenaltyRuns;
                    if (m.AwayTeamId == teamId && m.AwayPenaltyRuns > 0) runsConceded += m.AwayPenaltyRuns;
                }

                var points = won * 2 + tied + nr;
                double? nrr = null;
                if (oversFaced > 0 && oversBowled > 0)
                    nrr = Math.Round((runsScored / oversFaced) - (runsConceded / oversBowled), 3);

                rows.Add(new StandingRow
                {
                    TeamId = teamId,
                    GroupName = groupOf.GetValueOrDefault(teamId),
                    Team = teamMap.GetValueOrDefault(teamId),
                    MatchesPlayed = played, Wins = won, Losses = lost, Ties = tied, NoResults = nr,
                    Points = points,
                    NetRunRate = nrr,
                    RunsScored = (int)runsScored, OversFaced = Math.Round(oversFaced, 1),
                    RunsConceded = (int)runsConceded, OversBowled = Math.Round(oversBowled, 1),
                    Last5Form = form,
                });
            }

            // Sort: Points desc → NRR desc → Wins desc
            return rows
                .OrderByDescending(r => r.Points)
                .ThenByDescending(r => r.NetRunRate ?? double.MinValue)
                .ThenByDescending(r => r.Wins)
                .ToList();
        }

        [AllowAnonymous]
        [HttpGet("{id}/standings")]
        public async Task<IActionResult> Standings(int id)
        {
            var rows = await ComputeStandings(id);
            return Ok(rows);
        }

        // Top-N teams per pool — the set that advances to the knockout stage.
        // Returns a flat list of qualified teams (with their pool + seed).
        [AllowAnonymous]
        [HttpGet("{id}/qualifiers")]
        public async Task<IActionResult> Qualifiers(int id, [FromQuery] int perPool = 2)
        {
            var rows = await ComputeStandings(id);
            if (rows.Count == 0) return Ok(Array.Empty<object>());

            var result = new List<object>();
            var groups = rows
                .GroupBy(r => r.GroupName ?? "Pool A")
                .OrderBy(g => g.Key);
            foreach (var g in groups)
            {
                var ranked = g
                    .OrderByDescending(r => r.Points)
                    .ThenByDescending(r => r.NetRunRate ?? double.MinValue)
                    .ThenByDescending(r => r.Wins)
                    .Take(Math.Max(1, perPool))
                    .ToList();
                for (var i = 0; i < ranked.Count; i++)
                {
                    var r = ranked[i];
                    result.Add(new
                    {
                        r.TeamId,
                        r.Team,
                        Pool = g.Key,
                        Seed = i + 1,          // 1 = pool winner
                        SeedLabel = $"{g.Key} #{i + 1}",
                        r.Points,
                        r.NetRunRate,
                    });
                }
            }
            return Ok(result);
        }

        public class GenerateFixturesRequest
        {
            public bool DoubleRoundRobin { get; set; } = false;
            public int? StartingMatchNumber { get; set; }
            public bool ClearExisting { get; set; } = false;
            public DateTime? FirstMatchAt { get; set; }
            public int? DaysBetweenMatches { get; set; } = 1;
            public string? Venue { get; set; }
            public string? MatchFormat { get; set; }       // T10/T20/ODI/Custom
            public int? OversPerInnings { get; set; }
        }

        // Auto-generate match fixtures based on tournament format.
        [Authorize(Roles = "SuperAdmin,Admin,Scorer")]
        [HttpPost("{id}/generate-fixtures")]
        public async Task<IActionResult> GenerateFixtures(int id, [FromBody] GenerateFixturesRequest req)
        {
            var tournament = await _db.Tournaments.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (tournament == null) return NotFound();

            var tts = await _db.TournamentTeams
                .Where(tt => tt.TournamentId == id).ToListAsync();
            var teamIds = tts.Select(tt => tt.TeamId).ToList();
            if (teamIds.Count < 2) return BadRequest(new { message = "Tournament needs at least 2 teams." });

            if (req.ClearExisting)
            {
                var existing = await _db.Matches
                    .Where(m => m.TournamentId == id && !m.IsDeleted && m.MatchState == "Scheduled")
                    .ToListAsync();
                foreach (var m in existing) m.IsDeleted = true;
                await _db.SaveChangesAsync();
            }

            var format = tournament.Format ?? "RoundRobin";
            var matchFormat = req.MatchFormat ?? tournament.MatchFormat ?? "T20";
            var overs = req.OversPerInnings ?? tournament.OversPerInnings ?? 20;
            var startAt = req.FirstMatchAt ?? (tournament.StartDate ?? DateTime.UtcNow.AddDays(1));
            var daysGap = Math.Max(1, req.DaysBetweenMatches ?? 1);

            var fixtures = new List<(int home, int away, string stage)>();
            switch (format)
            {
                case "Knockout":
                    fixtures = BuildKnockoutBracket(teamIds);
                    break;
                case "Hybrid":
                    fixtures = BuildPoolFixtures(tts);
                    break;
                case "RoundRobin":
                default:
                    fixtures = BuildRoundRobin(teamIds, req.DoubleRoundRobin);
                    break;
            }

            var created = new List<Match>();
            for (var i = 0; i < fixtures.Count; i++)
            {
                var (home, away, stage) = fixtures[i];
                var m = new Match
                {
                    TournamentId = id,
                    MatchName = $"{tournament.Name} · Match {i + 1}",
                    HomeTeamId = home,
                    AwayTeamId = away,
                    ScheduledStart = startAt.AddDays(i * daysGap),
                    Venue = req.Venue,
                    MatchFormat = matchFormat,
                    OversPerInnings = overs,
                    StageLabel = stage,
                    MatchState = "Scheduled",
                    CreatedByUserId = User.GetUserId(),
                };
                _db.Matches.Add(m);
                created.Add(m);
            }
            await _db.SaveChangesAsync();
            return Ok(new { created = created.Count, fixtures = created });
        }

        // Knockout bracket view: arranges existing knockout matches into rounds.
        [AllowAnonymous]
        [HttpGet("{id}/brackets")]
        public async Task<IActionResult> Brackets(int id)
        {
            var matches = await _db.Matches
                .Where(m => m.TournamentId == id && !m.IsDeleted &&
                            (m.StageLabel != null && (m.StageLabel.Contains("Final") || m.StageLabel.Contains("Semi") || m.StageLabel.Contains("Quarter") || m.StageLabel.Contains("Eliminator"))))
                .OrderBy(m => m.ScheduledStart)
                .ToListAsync();

            var teamIds = matches.SelectMany(m => new[] { m.HomeTeamId, m.AwayTeamId }).Distinct().ToList();
            var teams = await _db.Teams.Where(t => teamIds.Contains(t.Id))
                .Select(t => new { t.Id, t.Name, t.ShortCode, t.LogoUrl }).ToListAsync();
            var teamMap = teams.ToDictionary(t => t.Id);

            var rounds = matches.GroupBy(m => m.StageLabel ?? "Final")
                                 .OrderBy(g => RoundOrder(g.Key))
                                 .Select(g => new {
                                     Round = g.Key,
                                     Matches = g.Select(m => new {
                                         m.Id, m.MatchName, m.ScheduledStart, m.MatchState,
                                         Home = teamMap.GetValueOrDefault(m.HomeTeamId),
                                         Away = teamMap.GetValueOrDefault(m.AwayTeamId),
                                         Winner = m.ResultWinnerTeamId.HasValue ? teamMap.GetValueOrDefault(m.ResultWinnerTeamId.Value) : null,
                                         m.ResultMargin,
                                     }).ToList()
                                 }).ToList();
            return Ok(rounds);
        }

        private static int RoundOrder(string stage) => stage switch
        {
            var s when s.Contains("Quarter") => 1,
            var s when s.Contains("Eliminator") => 2,
            var s when s.Contains("Semi") => 3,
            var s when s.Contains("Final") => 4,
            _ => 99,
        };

        // Single round-robin = circle method. Double = each team meets twice.
        private static List<(int, int, string)> BuildRoundRobin(List<int> teams, bool doubleRound)
        {
            var ids = new List<int>(teams);
            bool addedBye = false;
            if (ids.Count % 2 == 1) { ids.Add(-1); addedBye = true; } // bye placeholder

            var n = ids.Count;
            var matches = new List<(int, int, string)>();
            var rotation = new List<int>(ids);

            for (var round = 0; round < n - 1; round++)
            {
                for (var i = 0; i < n / 2; i++)
                {
                    var a = rotation[i];
                    var b = rotation[n - 1 - i];
                    if (addedBye && (a == -1 || b == -1)) continue;
                    matches.Add((a, b, $"Group Stage · Round {round + 1}"));
                }
                // Rotate: keep first fixed, rotate rest
                var last = rotation[n - 1];
                rotation.RemoveAt(n - 1);
                rotation.Insert(1, last);
            }
            if (doubleRound)
            {
                var second = matches.Select(t => (t.Item2, t.Item1, t.Item3.Replace("Round", "Return Round"))).ToList();
                matches.AddRange(second);
            }
            return matches;
        }

        // Power-of-two seeded knockout. 1 vs N, 2 vs N-1, …
        private static List<(int, int, string)> BuildKnockoutBracket(List<int> teams)
        {
            // Pad to next power of two with bye placeholders (-1). Byes auto-advance handled at match-record time.
            var size = 1; while (size < teams.Count) size *= 2;
            var padded = new List<int>(teams);
            while (padded.Count < size) padded.Add(-1);

            var label = size switch
            {
                <= 2  => "Final",
                <= 4  => "Semi-Final",
                <= 8  => "Quarter-Final",
                <= 16 => "Round of 16",
                _     => "Round of 32",
            };

            var matches = new List<(int, int, string)>();
            for (var i = 0; i < size / 2; i++)
            {
                var a = padded[i];
                var b = padded[size - 1 - i];
                if (a == -1 || b == -1) continue; // bye — skip generating this fixture
                matches.Add((a, b, label));
            }
            return matches;
        }

        // Pool stage: round-robin WITHIN each assigned pool (GroupName). Stage label = pool name.
        // Knockout fixtures (QF/SF/Final) are created manually afterwards from the qualifiers.
        // Falls back to auto 2-group split when no pools were assigned.
        private static List<(int, int, string)> BuildPoolFixtures(List<TournamentTeam> tts)
        {
            var assigned = tts.Where(t => !string.IsNullOrWhiteSpace(t.GroupName)).ToList();
            if (assigned.Count < 2)
                return BuildHybridFixtures(tts.Select(t => t.TeamId).ToList());

            var matches = new List<(int, int, string)>();
            foreach (var pool in assigned.GroupBy(t => t.GroupName!).OrderBy(g => g.Key))
            {
                var ids = pool.Select(t => t.TeamId).ToList();
                if (ids.Count < 2) continue;
                foreach (var (a, b, _) in BuildRoundRobin(ids, false))
                    matches.Add((a, b, pool.Key));
            }
            return matches;
        }

        // Group stage + knockout. Splits teams into 2 groups (or 1 if ≤4), round-robins each, top-2 advance to semis.
        private static List<(int, int, string)> BuildHybridFixtures(List<int> teams)
        {
            var matches = new List<(int, int, string)>();
            if (teams.Count <= 4)
            {
                matches.AddRange(BuildRoundRobin(teams, false));
                return matches;
            }
            var half = teams.Count / 2;
            var groupA = teams.Take(half).ToList();
            var groupB = teams.Skip(half).ToList();
            foreach (var (a, b, _) in BuildRoundRobin(groupA, false))
                matches.Add((a, b, "Group A"));
            foreach (var (a, b, _) in BuildRoundRobin(groupB, false))
                matches.Add((a, b, "Group B"));
            // Placeholders for SF/Final — actual teams filled after group stage by organizer.
            // We don't create the SF/Final matches here; organizer will add them after group stage.
            return matches;
        }

        // Aggregated leaderboards across every match in this tournament.
        [AllowAnonymous]
        [HttpGet("{id}/leaders")]
        public async Task<IActionResult> Leaders(int id, [FromQuery] int top = 5)
        {
            var matchIds = await _db.Matches
                .Where(m => m.TournamentId == id && !m.IsDeleted)
                .Select(m => m.Id).ToListAsync();
            if (matchIds.Count == 0)
                return Ok(new { runScorers = Array.Empty<object>(), wicketTakers = Array.Empty<object>(),
                                sixHitters = Array.Empty<object>(), fourHitters = Array.Empty<object>(),
                                bestInnings = Array.Empty<object>(), bestBowling = Array.Empty<object>(),
                                totals = new { matches = 0, runs = 0, wickets = 0, sixes = 0, fours = 0 } });

            var inningsIds = await _db.Innings.Where(i => matchIds.Contains(i.MatchId))
                                              .Select(i => i.Id).ToListAsync();

            var battingAll = await _db.BattingScores
                .Where(b => inningsIds.Contains(b.InningsId))
                .ToListAsync();
            var bowlingAll = await _db.BowlingFigures
                .Where(b => inningsIds.Contains(b.InningsId))
                .ToListAsync();

            var playerIds = battingAll.Select(b => b.PlayerId)
                                       .Union(bowlingAll.Select(b => b.PlayerId))
                                       .Distinct().ToList();
            var players = await _db.Players
                .Where(p => playerIds.Contains(p.Id))
                .Select(p => new { p.Id, p.FullName, p.TeamId, p.PhotoUrl, p.Role })
                .ToListAsync();
            var pmap = players.ToDictionary(p => p.Id);

            // Top run scorers
            var runScorers = battingAll
                .GroupBy(b => b.PlayerId)
                .Select(g => new {
                    PlayerId = g.Key,
                    Player = pmap.GetValueOrDefault(g.Key),
                    Runs = g.Sum(b => b.Runs),
                    Innings = g.Count(),
                    NotOuts = g.Count(b => !b.IsOut && b.BallsFaced > 0),
                    HighestScore = g.Max(b => b.Runs),
                    BallsFaced = g.Sum(b => b.BallsFaced),
                    Fours = g.Sum(b => b.Fours),
                    Sixes = g.Sum(b => b.Sixes),
                })
                .OrderByDescending(x => x.Runs).ThenByDescending(x => x.HighestScore)
                .Take(top).ToList();

            // Top wicket takers
            var wicketTakers = bowlingAll
                .GroupBy(b => b.PlayerId)
                .Select(g => new {
                    PlayerId = g.Key,
                    Player = pmap.GetValueOrDefault(g.Key),
                    Wickets = g.Sum(b => b.Wickets),
                    Runs = g.Sum(b => b.RunsConceded),
                    LegalBalls = g.Sum(b => b.LegalBalls),
                    Economy = g.Sum(b => b.LegalBalls) == 0 ? 0.0
                              : Math.Round(g.Sum(b => b.RunsConceded) / (g.Sum(b => b.LegalBalls) / 6.0), 2),
                })
                .OrderByDescending(x => x.Wickets).ThenBy(x => x.Economy)
                .Take(top).ToList();

            // Six hitters
            var sixHitters = battingAll
                .GroupBy(b => b.PlayerId)
                .Select(g => new { PlayerId = g.Key, Player = pmap.GetValueOrDefault(g.Key), Sixes = g.Sum(b => b.Sixes) })
                .OrderByDescending(x => x.Sixes).Take(top).ToList();

            // Four hitters
            var fourHitters = battingAll
                .GroupBy(b => b.PlayerId)
                .Select(g => new { PlayerId = g.Key, Player = pmap.GetValueOrDefault(g.Key), Fours = g.Sum(b => b.Fours) })
                .OrderByDescending(x => x.Fours).Take(top).ToList();

            // Best individual innings
            var bestInnings = battingAll
                .OrderByDescending(b => b.Runs).ThenBy(b => b.BallsFaced)
                .Take(top)
                .Select(b => new {
                    Player = pmap.GetValueOrDefault(b.PlayerId),
                    b.Runs, b.BallsFaced, b.Fours, b.Sixes,
                    StrikeRate = b.BallsFaced == 0 ? 0.0 : Math.Round((b.Runs / (double)b.BallsFaced) * 100, 2)
                }).ToList();

            // Best bowling figures (single innings)
            var bestBowling = bowlingAll
                .OrderByDescending(b => b.Wickets).ThenBy(b => b.RunsConceded)
                .Take(top)
                .Select(b => new {
                    Player = pmap.GetValueOrDefault(b.PlayerId),
                    Figures = $"{b.Wickets}/{b.RunsConceded}",
                    Overs = $"{b.LegalBalls / 6}.{b.LegalBalls % 6}"
                }).ToList();

            // Tournament totals
            var totalRuns = battingAll.Sum(b => b.Runs);
            var totalWickets = bowlingAll.Sum(b => b.Wickets);
            var totalSixes = battingAll.Sum(b => b.Sixes);
            var totalFours = battingAll.Sum(b => b.Fours);

            return Ok(new {
                runScorers, wicketTakers, sixHitters, fourHitters,
                bestInnings, bestBowling,
                totals = new { matches = matchIds.Count, runs = totalRuns, wickets = totalWickets, sixes = totalSixes, fours = totalFours }
            });
        }

        public class StageRequest { public string Stage { get; set; } = ""; }

        [Authorize(Roles = "SuperAdmin,Scorer")]
        [HttpPut("{id}/stage")]
        public async Task<IActionResult> UpdateStage(int id, [FromBody] StageRequest req)
        {
            var t = await _db.Tournaments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (t == null) return NotFound();
            t.Stage = req.Stage;
            t.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(t);
        }
    }
}
