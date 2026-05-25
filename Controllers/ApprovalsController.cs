using GBS.Api.Data;
using GBS.Api.DbModels;
using GBS.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    [ApiController]
    [Route("api/approvals")]
    public class ApprovalsController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public ApprovalsController(GBS_DbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] string status = "Pending", [FromQuery] string? entityType = null)
        {
            var q = _db.ApprovalRequests.Where(a => a.Status == status);
            if (!string.IsNullOrWhiteSpace(entityType)) q = q.Where(a => a.EntityType == entityType);

            var requests = await q.OrderByDescending(a => a.CreatedAt).ToListAsync();

            // Hydrate entity details for the SuperAdmin feed
            var enriched = new List<object>();
            foreach (var r in requests)
            {
                object? detail = r.EntityType switch
                {
                    "User"   => await _db.Users.Where(u => u.Id == r.EntityId).Select(u => (object)new { u.Id, u.Username, u.FullName, u.Role, u.Email, u.Phone, u.CreatedAt }).FirstOrDefaultAsync(),
                    "Team"   => await _db.Teams.Where(t => t.Id == r.EntityId).Select(t => (object)new { t.Id, t.Name, t.ShortCode, t.Category, t.City, t.LogoUrl, t.CaptainUserId }).FirstOrDefaultAsync(),
                    "Player" => await _db.Players.Where(p => p.Id == r.EntityId).Select(p => (object)new { p.Id, p.FullName, p.Role, p.TeamId, p.PhotoUrl, p.City, p.JerseyNumber }).FirstOrDefaultAsync(),
                    _ => null
                };
                enriched.Add(new { request = r, entity = detail });
            }
            return Ok(enriched);
        }

        public class ReviewRequest { public string? RejectionReason { get; set; } }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var r = await _db.ApprovalRequests.FindAsync(id);
            if (r == null) return NotFound();
            if (r.Status != "Pending") return BadRequest(new { message = "Already reviewed." });

            r.Status = "Approved";
            r.ReviewedByUserId = User.GetUserId();
            r.ReviewedAt = DateTime.UtcNow;

            switch (r.EntityType)
            {
                case "User":
                    var u = await _db.Users.FindAsync(r.EntityId);
                    if (u != null) { u.ApprovalStatus = "Approved"; u.ApprovedByUserId = User.GetUserId(); u.ApprovedAt = DateTime.UtcNow; }
                    break;
                case "Team":
                    var t = await _db.Teams.FindAsync(r.EntityId);
                    if (t != null) { t.ApprovalStatus = "Approved"; t.ApprovedByUserId = User.GetUserId(); t.ApprovedAt = DateTime.UtcNow; }
                    break;
                case "Player":
                    var p = await _db.Players.FindAsync(r.EntityId);
                    if (p != null) { p.ApprovalStatus = "Approved"; p.ApprovedByUserId = User.GetUserId(); p.ApprovedAt = DateTime.UtcNow; }
                    break;
            }

            await _db.SaveChangesAsync();
            return Ok(r);
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] ReviewRequest req)
        {
            var r = await _db.ApprovalRequests.FindAsync(id);
            if (r == null) return NotFound();
            if (r.Status != "Pending") return BadRequest(new { message = "Already reviewed." });

            r.Status = "Rejected";
            r.RejectionReason = req.RejectionReason;
            r.ReviewedByUserId = User.GetUserId();
            r.ReviewedAt = DateTime.UtcNow;

            switch (r.EntityType)
            {
                case "User":
                    var u = await _db.Users.FindAsync(r.EntityId);
                    if (u != null) u.ApprovalStatus = "Rejected";
                    break;
                case "Team":
                    var t = await _db.Teams.FindAsync(r.EntityId);
                    if (t != null) t.ApprovalStatus = "Rejected";
                    break;
                case "Player":
                    var p = await _db.Players.FindAsync(r.EntityId);
                    if (p != null) p.ApprovalStatus = "Rejected";
                    break;
            }

            await _db.SaveChangesAsync();
            return Ok(r);
        }
    }
}
