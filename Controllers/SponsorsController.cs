using GBS.Api.Data;
using GBS.Api.DbModels;
using GBS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    [ApiController]
    [Route("api/sponsors")]
    public class SponsorsController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public SponsorsController(GBS_DbContext db) { _db = db; }

        public class SponsorRequest
        {
            public string Name { get; set; } = "";
            public string? Tagline { get; set; }
            public string? WebsiteUrl { get; set; }
            public string? ContactPhone { get; set; }
            public string? Slots { get; set; }
            public int? TournamentId { get; set; }
            public bool IsActive { get; set; } = true;
            public string? LogoBase64 { get; set; }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string? slot, [FromQuery] int? tournamentId)
        {
            var q = _db.Sponsors.Where(s => !s.IsDeleted && s.IsActive);
            if (tournamentId.HasValue) q = q.Where(s => s.TournamentId == tournamentId || s.TournamentId == null);
            var sponsors = await q.OrderBy(s => s.Name).ToListAsync();
            if (!string.IsNullOrWhiteSpace(slot))
                sponsors = sponsors.Where(s => (s.Slots ?? "").Split(',').Select(x => x.Trim()).Contains(slot)).ToList();
            return Ok(sponsors);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var s = await _db.Sponsors.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (s == null) return NotFound();
            return Ok(s);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SponsorRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { message = "Name required." });
            var s = new Sponsor
            {
                Name = req.Name.Trim(),
                Tagline = req.Tagline,
                WebsiteUrl = req.WebsiteUrl,
                ContactPhone = req.ContactPhone,
                Slots = req.Slots,
                TournamentId = req.TournamentId,
                IsActive = req.IsActive,
            };
            if (!string.IsNullOrEmpty(req.LogoBase64))
            {
                var ext = req.LogoBase64.Contains("image/png") ? ".png" : ".jpg";
                s.LogoUrl = IOHelper.SaveFile(req.LogoBase64, $"sponsor_{Guid.NewGuid():N}{ext}", "sponsors");
            }
            _db.Sponsors.Add(s);
            await _db.SaveChangesAsync();
            return Ok(s);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SponsorRequest req)
        {
            var s = await _db.Sponsors.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (s == null) return NotFound();
            s.Name = req.Name ?? s.Name;
            s.Tagline = req.Tagline ?? s.Tagline;
            s.WebsiteUrl = req.WebsiteUrl ?? s.WebsiteUrl;
            s.ContactPhone = req.ContactPhone ?? s.ContactPhone;
            s.Slots = req.Slots ?? s.Slots;
            s.TournamentId = req.TournamentId ?? s.TournamentId;
            s.IsActive = req.IsActive;
            if (!string.IsNullOrEmpty(req.LogoBase64))
            {
                var ext = req.LogoBase64.Contains("image/png") ? ".png" : ".jpg";
                s.LogoUrl = IOHelper.SaveFile(req.LogoBase64, $"sponsor_{Guid.NewGuid():N}{ext}", "sponsors");
            }
            s.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return Ok(s);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _db.Sponsors.FindAsync(id);
            if (s == null) return NotFound();
            s.IsDeleted = true;
            s.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
