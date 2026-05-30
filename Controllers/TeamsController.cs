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
    [Route("api/teams")]
    public class TeamsController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public TeamsController(GBS_DbContext db) { _db = db; }

        public class TeamRequest
        {
            public string Name { get; set; } = "";
            public string ShortCode { get; set; } = "";
            public string? Category { get; set; }
            public string? City { get; set; }
            public string? HomeVenue { get; set; }
            public short? FoundedYear { get; set; }
            public string? PrimaryColorHex { get; set; }
            public string? SecondaryColorHex { get; set; }
            public int? CaptainUserId { get; set; } // user to assign as Captain
            // Optional base64-encoded photo ("data:image/png;base64,...")
            public string? LogoBase64 { get; set; }
        }

        public class AssignCaptainRequest { public int CaptainUserId { get; set; } }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("{id}/captain")]
        public async Task<IActionResult> AssignCaptain(int id, [FromBody] AssignCaptainRequest req)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (team == null) return NotFound();
            var user = await _db.Users.FindAsync(req.CaptainUserId);
            if (user == null) return NotFound(new { message = "User not found." });

            team.CaptainUserId = user.Id;
            team.UpdatedAt = DateTime.UtcNow;
            user.TeamId = team.Id;
            if (user.Role != "SuperAdmin" && user.Role != "Admin") user.Role = "Captain";
            if (user.ApprovalStatus != "Approved")
            {
                user.ApprovalStatus = "Approved";
                user.ApprovedByUserId = User.GetUserId();
                user.ApprovedAt = DateTime.UtcNow;
            }
            await _db.SaveChangesAsync();
            return Ok(new { team, captain = new { user.Id, user.Username, user.FullName, user.Role } });
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? search, [FromQuery] string? category, [FromQuery] string? status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var q = _db.Teams.Where(t => !t.IsDeleted);
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(t => t.Name.Contains(search) || t.ShortCode.Contains(search) || (t.City != null && t.City.Contains(search)));
            if (!string.IsNullOrWhiteSpace(category)) q = q.Where(t => t.Category == category);
            if (!string.IsNullOrWhiteSpace(status))   q = q.Where(t => t.ApprovalStatus == status);

            // Public listing only shows Approved unless user is SuperAdmin
            if (User.GetRole() != "SuperAdmin" && string.IsNullOrWhiteSpace(status))
                q = q.Where(t => t.ApprovalStatus == "Approved");

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(t => t.CreatedAt)
                               .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { items, page, pageSize, totalCount = total, totalPages = (int)Math.Ceiling(total / (double)pageSize) });
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (team == null) return NotFound();
            var roster = await (from tp in _db.TeamPlayers
                                join p in _db.Players on tp.PlayerId equals p.Id
                                where tp.TeamId == id && tp.IsActive && !p.IsDeleted
                                select new { p.Id, p.FullName, p.Role, p.PhotoUrl, tp.JerseyNumber, p.ApprovalStatus }).ToListAsync();
            return Ok(new { team, roster });
        }

        // Only SuperAdmin (or Admin role) creates teams. Captains are assigned to existing teams.
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeamRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.ShortCode))
                return BadRequest(new { message = "Name and ShortCode required." });

            var shortCode = req.ShortCode.Trim().ToUpperInvariant();
            if (shortCode.Length > 4)
                return BadRequest(new { message = "ShortCode must be 4 characters or fewer." });

            // The unique index on ShortCode spans ALL rows (including soft-deleted),
            // so check without the IsDeleted filter to avoid a DB constraint crash.
            var clash = await _db.Teams.FirstOrDefaultAsync(t => t.ShortCode == shortCode);
            if (clash != null)
            {
                if (!clash.IsDeleted)
                    return Conflict(new { message = "ShortCode already exists." });

                // Reuse the soft-deleted slot instead of inserting a duplicate code.
                return await ReviveDeletedTeam(clash, req, shortCode);
            }

            var userId = User.GetUserId();

            var team = new Team
            {
                Name = req.Name.Trim(),
                ShortCode = shortCode,
                Category = req.Category,
                City = req.City,
                HomeVenue = req.HomeVenue,
                FoundedYear = req.FoundedYear,
                PrimaryColorHex = req.PrimaryColorHex,
                SecondaryColorHex = req.SecondaryColorHex,
                CaptainUserId = req.CaptainUserId,
                ApprovalStatus = "Approved",        // Admin-created teams are pre-approved
                ApprovedByUserId = userId,
                ApprovedAt = DateTime.UtcNow
            };

            // If a captain user is assigned, link them to this team and ensure their role is Captain.
            if (req.CaptainUserId.HasValue)
            {
                var cap = await _db.Users.FirstOrDefaultAsync(u => u.Id == req.CaptainUserId);
                if (cap != null)
                {
                    cap.TeamId = null; // will set after team gets an Id below
                    if (cap.Role != "SuperAdmin" && cap.Role != "Admin") cap.Role = "Captain";
                    if (cap.ApprovalStatus != "Approved") {
                        cap.ApprovalStatus = "Approved";
                        cap.ApprovedByUserId = userId;
                        cap.ApprovedAt = DateTime.UtcNow;
                    }
                }
            }

            var (logoUrl, logoErr) = TrySaveLogo(req.LogoBase64);
            if (logoErr != null) return BadRequest(new { message = logoErr });
            if (logoUrl != null) team.LogoUrl = logoUrl;

            _db.Teams.Add(team);
            try
            {
                await _db.SaveChangesAsync();

                // Now that team has an Id, link the captain user to it.
                if (req.CaptainUserId.HasValue)
                {
                    var cap = await _db.Users.FirstOrDefaultAsync(u => u.Id == req.CaptainUserId);
                    if (cap != null) { cap.TeamId = team.Id; await _db.SaveChangesAsync(); }
                }
            }
            catch (DbUpdateException ex)
            {
                return Conflict(new { message = "Could not save team: " + (ex.InnerException?.Message ?? ex.Message) });
            }

            return Ok(team);
        }

        // Restores a previously soft-deleted team that owns the requested ShortCode,
        // so the unique index is respected instead of throwing a 500.
        private async Task<IActionResult> ReviveDeletedTeam(Team team, TeamRequest req, string shortCode)
        {
            var userId = User.GetUserId();
            team.IsDeleted = false;
            team.Name = req.Name.Trim();
            team.ShortCode = shortCode;
            team.Category = req.Category;
            team.City = req.City;
            team.HomeVenue = req.HomeVenue;
            team.FoundedYear = req.FoundedYear;
            team.PrimaryColorHex = req.PrimaryColorHex;
            team.SecondaryColorHex = req.SecondaryColorHex;
            team.CaptainUserId = req.CaptainUserId;
            team.ApprovalStatus = "Approved";
            team.ApprovedByUserId = userId;
            team.ApprovedAt = DateTime.UtcNow;
            team.UpdatedAt = DateTime.UtcNow;

            var (logoUrl, logoErr) = TrySaveLogo(req.LogoBase64);
            if (logoErr != null) return BadRequest(new { message = logoErr });
            if (logoUrl != null) team.LogoUrl = logoUrl;

            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException ex)
            {
                return Conflict(new { message = "Could not save team: " + (ex.InnerException?.Message ?? ex.Message) });
            }
            return Ok(team);
        }

        // Returns (savedUrl, error). Never throws on malformed input.
        private static (string?, string?) TrySaveLogo(string? logoBase64)
        {
            if (string.IsNullOrEmpty(logoBase64)) return (null, null);
            try
            {
                var ext = logoBase64.Contains("image/png") ? ".png" : ".jpg";
                var url = IOHelper.SaveFile(logoBase64, $"team_{Guid.NewGuid():N}{ext}", "teams");
                return (url, null);
            }
            catch
            {
                return (null, "Invalid logo image data.");
            }
        }

        [Authorize(Roles = "SuperAdmin,Admin,Captain")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TeamRequest req)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (team == null) return NotFound();

            var role = User.GetRole();
            var userId = User.GetUserId();
            if (role == "Captain" && team.CaptainUserId != userId) return Forbid();

            team.Name = req.Name ?? team.Name;
            team.Category = req.Category ?? team.Category;
            team.City = req.City ?? team.City;
            team.HomeVenue = req.HomeVenue ?? team.HomeVenue;
            team.FoundedYear = req.FoundedYear ?? team.FoundedYear;
            team.PrimaryColorHex = req.PrimaryColorHex ?? team.PrimaryColorHex;
            team.SecondaryColorHex = req.SecondaryColorHex ?? team.SecondaryColorHex;
            team.UpdatedAt = DateTime.UtcNow;

            var (logoUrl, logoErr) = TrySaveLogo(req.LogoBase64);
            if (logoErr != null) return BadRequest(new { message = logoErr });
            if (logoUrl != null) team.LogoUrl = logoUrl;

            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException ex)
            {
                return Conflict(new { message = "Could not save team: " + (ex.InnerException?.Message ?? ex.Message) });
            }
            return Ok(team);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _db.Teams.FindAsync(id);
            if (team == null) return NotFound();
            team.IsDeleted = true;
            team.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // Roster management - captain adds players to own team
        public class AddPlayerRequest { public int PlayerId { get; set; } public int? JerseyNumber { get; set; } public string? Season { get; set; } }

        [Authorize(Roles = "SuperAdmin,Captain")]
        [HttpPost("{id}/players")]
        public async Task<IActionResult> AddPlayer(int id, [FromBody] AddPlayerRequest req)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (team == null) return NotFound();

            var role = User.GetRole();
            var userId = User.GetUserId();
            if (role == "Captain" && team.CaptainUserId != userId) return Forbid();

            var player = await _db.Players.FirstOrDefaultAsync(p => p.Id == req.PlayerId && !p.IsDeleted);
            if (player == null) return NotFound(new { message = "Player not found." });

            var exists = await _db.TeamPlayers.AnyAsync(tp => tp.TeamId == id && tp.PlayerId == req.PlayerId && tp.IsActive);
            if (exists) return Conflict(new { message = "Player already in team." });

            _db.TeamPlayers.Add(new TeamPlayer
            {
                TeamId = id, PlayerId = req.PlayerId, JerseyNumber = req.JerseyNumber, Season = req.Season, IsActive = true
            });
            await _db.SaveChangesAsync();
            return Ok(new { message = "Player added." });
        }

        [Authorize(Roles = "SuperAdmin,Captain")]
        [HttpDelete("{id}/players/{playerId}")]
        public async Task<IActionResult> RemovePlayer(int id, int playerId)
        {
            var team = await _db.Teams.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (team == null) return NotFound();

            var role = User.GetRole();
            var userId = User.GetUserId();
            if (role == "Captain" && team.CaptainUserId != userId) return Forbid();

            var tp = await _db.TeamPlayers.FirstOrDefaultAsync(x => x.TeamId == id && x.PlayerId == playerId && x.IsActive);
            if (tp == null) return NotFound();
            tp.IsActive = false;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
