using GBS.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    // Global, all-time player & team rankings for Imran Sidhu Memorial VCC.
    // Ratings accumulate across every completed match (all tournaments).
    [ApiController]
    [Route("api/rankings")]
    public class RankingsController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public RankingsController(GBS_DbContext db) { _db = db; }

        // ---------- shared loaders ----------
        private async Task<(List<int> matchIds,
                            Dictionary<int, DbModels.Match> matches,
                            List<DbModels.Innings> innings,
                            Dictionary<int, DbModels.Innings> inningsById)> LoadCoreAsync()
        {
            var matches = await _db.Matches
                .Where(m => !m.IsDeleted && m.MatchState == "Completed")
                .ToListAsync();
            var matchIds = matches.Select(m => m.Id).ToList();
            var innings = matchIds.Count == 0
                ? new List<DbModels.Innings>()
                : await _db.Innings.Where(i => matchIds.Contains(i.MatchId)).ToListAsync();
            return (matchIds, matches.ToDictionary(m => m.Id), innings,
                    innings.ToDictionary(i => i.Id));
        }

        // ============================================================
        //  BATSMEN RATING (0–1000)
        //  Points/innings = Runs + %ofTeamTotal + (StrikeRate*0.25)
        //  + MoM bonus(100/match).  Rating = TotalPoints / TotalInnings
        //  (not-out innings add points but are NOT counted in divisor).
        //  Missing-match penalty: -1% of rating per team match missed.
        // ============================================================
        [AllowAnonymous]
        [HttpGet("batting")]
        public async Task<IActionResult> Batting([FromQuery] int top = 100)
        {
            var (matchIds, matches, innings, inningsById) = await LoadCoreAsync();
            if (matchIds.Count == 0) return Ok(Array.Empty<object>());

            var inningsIds = innings.Select(i => i.Id).ToList();
            var batting = await _db.BattingScores
                .Where(b => inningsIds.Contains(b.InningsId)).ToListAsync();

            // matchId per innings, for MoM + matches-played + team-matches
            var matchIdOfInnings = innings.ToDictionary(i => i.Id, i => i.MatchId);

            var grouped = batting.GroupBy(b => b.PlayerId).Select(g =>
            {
                double pts = 0; int outs = 0;
                foreach (var b in g)
                {
                    var inn = inningsById.GetValueOrDefault(b.InningsId);
                    var teamTotal = inn?.TotalRuns ?? 0;
                    var pctOfTeam = teamTotal > 0 ? (b.Runs / (double)teamTotal) * 100.0 : 0.0;
                    var sr = b.BallsFaced > 0 ? (b.Runs / (double)b.BallsFaced) * 100.0 : 0.0;
                    pts += b.Runs + pctOfTeam + sr * 0.25;
                    if (b.IsOut) outs++;
                }
                var playedMatchIds = g.Select(b => matchIdOfInnings.GetValueOrDefault(b.InningsId))
                                      .Distinct().ToList();
                int momBonus = playedMatchIds.Count(mid =>
                    matches.GetValueOrDefault(mid)?.ManOfTheMatchPlayerId == g.Key) * 100;
                var total = pts + momBonus;
                var divisor = outs > 0 ? outs : 1; // exclude not-outs; guard /0
                var rating = total / divisor;
                return new { PlayerId = g.Key, Rating = rating,
                             Innings = g.Count(), Outs = outs,
                             PlayedMatchIds = playedMatchIds };
            }).ToList();

            // team-match counts for missing-match penalty
            var playerMeta = await _db.Players
                .Where(p => grouped.Select(x => x.PlayerId).Contains(p.Id))
                .Select(p => new { p.Id, p.FullName, p.TeamId, p.PhotoUrl }).ToListAsync();
            var pmap = playerMeta.ToDictionary(p => p.Id);

            int TeamMatches(int? teamId) => teamId == null ? 0
                : matches.Values.Count(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId);

            var result = grouped.Select(x =>
            {
                var p = pmap.GetValueOrDefault(x.PlayerId);
                var teamMatches = TeamMatches(p?.TeamId);
                var missed = Math.Max(0, teamMatches - x.PlayedMatchIds.Count);
                var rating = x.Rating * Math.Max(0, 1 - 0.01 * missed);
                rating = Math.Clamp(rating, 0, 1000);
                return new {
                    playerId = x.PlayerId,
                    name = p?.FullName ?? "Unknown",
                    photoUrl = p?.PhotoUrl,
                    teamId = p?.TeamId,
                    rating = Math.Round(rating, 1),
                    innings = x.Innings,
                    notOuts = x.Innings - x.Outs,
                    matches = x.PlayedMatchIds.Count,
                    missed,
                };
            })
            .OrderByDescending(r => r.rating).ThenByDescending(r => r.matches)
            .Take(top).ToList();

            return Ok(result);
        }

        // ============================================================
        //  BOWLER RATING
        //  Points = AvgRunsPoints + (Wickets*30) + HaulBonus(3/4/5)
        //         + WicketMaidenOver*10 + MoM(100/match)
        //  Rating = TotalPoints / TotalMatches bowled.
        // ============================================================
        [AllowAnonymous]
        [HttpGet("bowling")]
        public async Task<IActionResult> Bowling([FromQuery] int top = 100)
        {
            var (matchIds, matches, innings, inningsById) = await LoadCoreAsync();
            if (matchIds.Count == 0) return Ok(Array.Empty<object>());

            var inningsIds = innings.Select(i => i.Id).ToList();
            var bowling = await _db.BowlingFigures
                .Where(b => inningsIds.Contains(b.InningsId)).ToListAsync();
            var balls = await _db.Balls
                .Where(b => matchIds.Contains(b.MatchId) && !b.IsUndone && b.BowlerPlayerId != null)
                .ToListAsync();
            var matchIdOfInnings = innings.ToDictionary(i => i.Id, i => i.MatchId);

            // Wicket-maiden overs per bowler: an over with 0 charged runs and >=1 bowler wicket.
            var wicketMaidens = balls
                .GroupBy(b => new { b.BowlerPlayerId, b.InningsId, b.OverNumber })
                .Where(g =>
                {
                    int charged = g.Sum(x => x.RunsBatter)
                        + g.Where(x => x.ExtrasType == "Wide" || x.ExtrasType == "NoBall")
                            .Sum(x => x.RunsExtras);
                    int wkts = g.Count(x => x.IsWicket && !IsRunOut(x.WicketType));
                    return charged == 0 && wkts >= 1;
                })
                .GroupBy(g => g.Key.BowlerPlayerId!.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var grouped = bowling.GroupBy(b => b.PlayerId).Select(g =>
            {
                int wickets = g.Sum(b => b.Wickets);
                int runs = g.Sum(b => b.RunsConceded);
                double avgPoints = AvgRunsPoints(wickets, runs);
                int haulBonus = g.Sum(b => b.Wickets >= 5 ? 50 : b.Wickets == 4 ? 40 : b.Wickets >= 3 ? 30 : 0);
                int wmBonus = wicketMaidens.GetValueOrDefault(g.Key) * 10;
                var playedMatchIds = g.Select(b => matchIdOfInnings.GetValueOrDefault(b.InningsId))
                                      .Distinct().ToList();
                int momBonus = playedMatchIds.Count(mid =>
                    matches.GetValueOrDefault(mid)?.ManOfTheMatchPlayerId == g.Key) * 100;
                double total = avgPoints + wickets * 30 + haulBonus + wmBonus + momBonus;
                var divisor = playedMatchIds.Count > 0 ? playedMatchIds.Count : 1;
                return new { PlayerId = g.Key, Rating = total / divisor,
                             Wickets = wickets, Runs = runs, Matches = playedMatchIds.Count,
                             Average = wickets > 0 ? Math.Round(runs / (double)wickets, 2) : 0.0 };
            }).ToList();

            var playerMeta = await _db.Players
                .Where(p => grouped.Select(x => x.PlayerId).Contains(p.Id))
                .Select(p => new { p.Id, p.FullName, p.TeamId, p.PhotoUrl }).ToListAsync();
            var pmap = playerMeta.ToDictionary(p => p.Id);

            var result = grouped.Select(x =>
            {
                var p = pmap.GetValueOrDefault(x.PlayerId);
                return new {
                    playerId = x.PlayerId,
                    name = p?.FullName ?? "Unknown",
                    photoUrl = p?.PhotoUrl,
                    teamId = p?.TeamId,
                    rating = Math.Round(x.Rating, 1),
                    wickets = x.Wickets,
                    average = x.Average,
                    matches = x.Matches,
                };
            })
            .OrderByDescending(r => r.rating).ThenByDescending(r => r.wickets)
            .Take(top).ToList();

            return Ok(result);
        }

        // ============================================================
        //  TEAM RANKING
        //  Start 500. Per match: Win +50+RRP, Loss -50+RRP (RRP=NRR*10,
        //  NRR = teamRR - oppRR). Stage bonuses QF20/SF30/Final50/Champ100.
        //  Rating = TotalPoints / TotalMatches.
        // ============================================================
        [AllowAnonymous]
        [HttpGet("teams")]
        public async Task<IActionResult> Teams()
        {
            var (matchIds, matches, innings, inningsById) = await LoadCoreAsync();
            var teams = await _db.Teams.Where(t => !t.IsDeleted)
                .Select(t => new { t.Id, t.Name, t.ShortCode, t.LogoUrl }).ToListAsync();

            // run rate per (matchId, teamId)
            double RunRate(DbModels.Innings? inn)
            {
                if (inn == null || inn.LegalBallsBowled == 0) return 0;
                return inn.TotalRuns / (inn.LegalBallsBowled / 6.0);
            }

            var acc = teams.ToDictionary(t => t.Id, t => new TeamAcc());

            foreach (var m in matches.Values)
            {
                var homeInn = innings.FirstOrDefault(i => i.MatchId == m.Id && i.BattingTeamId == m.HomeTeamId);
                var awayInn = innings.FirstOrDefault(i => i.MatchId == m.Id && i.BattingTeamId == m.AwayTeamId);
                double homeRR = RunRate(homeInn), awayRR = RunRate(awayInn);

                void Apply(int teamId, double rr, double oppRR)
                {
                    if (!acc.ContainsKey(teamId)) return;
                    var a = acc[teamId];
                    a.Matches++;
                    double rrp = (rr - oppRR) * 10.0;
                    if (m.ResultWinnerTeamId == teamId) a.Points += 50 + rrp;
                    else if (m.ResultWinnerTeamId != null) a.Points += -50 + rrp;
                    // no-result: no win/loss points
                    AddStageBonus(a, m, teamId);
                }
                Apply(m.HomeTeamId, homeRR, awayRR);
                Apply(m.AwayTeamId, awayRR, homeRR);
            }

            var result = teams.Select(t =>
            {
                var a = acc[t.Id];
                double total = 500 + a.Points + a.StageBonus;
                double rating = a.Matches > 0 ? total / a.Matches : total;
                return new {
                    teamId = t.Id, name = t.Name, shortCode = t.ShortCode, logoUrl = t.LogoUrl,
                    rating = Math.Round(rating, 1),
                    points = Math.Round(total, 1),
                    matches = a.Matches,
                };
            })
            .Where(r => r.matches > 0)
            .OrderByDescending(r => r.rating).ThenByDescending(r => r.points)
            .ToList();

            return Ok(result);
        }

        // ---------- helpers ----------
        private class TeamAcc
        {
            public double Points; public int Matches; public double StageBonus;
            public bool HasQF, HasSF, HasFinal, HasChamp;
        }

        private static void AddStageBonus(TeamAcc a, DbModels.Match m, int teamId)
        {
            var label = (m.StageLabel ?? "").ToLowerInvariant();
            // Accumulate distinct stage participation flags
            if (label.Contains("quarter") && !a.HasQF) { a.StageBonus += 20; a.HasQF = true; }
            if (label.Contains("semi") && !a.HasSF) { a.StageBonus += 30; a.HasSF = true; }
            bool isFinal = label.Contains("final") && !label.Contains("quarter") && !label.Contains("semi");
            if (isFinal && !a.HasFinal) { a.StageBonus += 50; a.HasFinal = true; }
            if (isFinal && m.ResultWinnerTeamId == teamId && !a.HasChamp) { a.StageBonus += 100; a.HasChamp = true; }
        }

        private static double AvgRunsPoints(int wickets, int runs)
        {
            if (wickets == 0) return 0;
            double avg = runs / (double)wickets;
            if (avg > 15) return 0;
            int r = (int)Math.Clamp(Math.Round(avg, MidpointRounding.AwayFromZero), 0, 15);
            return r == 0 ? 90 : (16 - r) * 5;
        }

        private static bool IsRunOut(string? wicketType)
        {
            if (string.IsNullOrEmpty(wicketType)) return false;
            var w = wicketType.Replace(" ", "").ToLowerInvariant();
            return w == "runout";
        }
    }
}
