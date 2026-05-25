using GBS.Api.Data;
using GBS.Api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public NotificationsController(GBS_DbContext db) { _db = db; }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var uid = User.GetUserId();
            var q = _db.Notifications.Where(n => n.UserId == null || n.UserId == uid);
            var total = await q.CountAsync();
            var items = await q.OrderByDescending(n => n.CreatedAt)
                                .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var unread = await q.CountAsync(n => !n.IsRead);
            return Ok(new { items, page, pageSize, totalCount = total, unread });
        }

        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkRead(int id)
        {
            var n = await _db.Notifications.FindAsync(id);
            if (n == null) return NotFound();
            n.IsRead = true;
            await _db.SaveChangesAsync();
            return Ok(n);
        }

        [HttpPost("read-all")]
        public async Task<IActionResult> MarkAllRead()
        {
            var uid = User.GetUserId();
            var items = await _db.Notifications
                .Where(n => (n.UserId == null || n.UserId == uid) && !n.IsRead)
                .ToListAsync();
            foreach (var n in items) n.IsRead = true;
            await _db.SaveChangesAsync();
            return Ok(new { updated = items.Count });
        }
    }
}
