using GBS.Api.Authorization;
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
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        public UsersController(GBS_DbContext db) { _db = db; }

        // ============ Current user ============

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var id = User.GetUserId();
            if (id == null) return Unauthorized();
            var u = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return NotFound();
            return Ok(Sanitize(u));
        }

        public class UpdateMeRequest
        {
            public string? FullName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? AvatarBase64 { get; set; }
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateMeRequest req)
        {
            var id = User.GetUserId();
            var u = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return NotFound();

            u.FullName = req.FullName ?? u.FullName;
            u.Email = req.Email ?? u.Email;
            u.Phone = req.Phone ?? u.Phone;
            if (!string.IsNullOrEmpty(req.AvatarBase64))
            {
                var ext = req.AvatarBase64.Contains("image/png") ? ".png" : ".jpg";
                u.AvatarUrl = IOHelper.SaveFile(req.AvatarBase64, $"user_{Guid.NewGuid():N}{ext}", "users");
            }
            await _db.SaveChangesAsync();
            return Ok(Sanitize(u));
        }

        public class ChangePasswordRequest
        {
            public string OldPassword { get; set; } = "";
            public string NewPassword { get; set; } = "";
        }

        [HttpPost("me/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest req)
        {
            var id = User.GetUserId();
            var u = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (u == null) return NotFound();
            if (u.Password != req.OldPassword) return BadRequest(new { message = "Old password incorrect." });
            if (string.IsNullOrWhiteSpace(req.NewPassword) || req.NewPassword.Length < 6)
                return BadRequest(new { message = "New password must be at least 6 characters." });
            u.Password = req.NewPassword;
            await _db.SaveChangesAsync();
            return Ok(new { message = "Password updated." });
        }

        // ============ SuperAdmin user management ============

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> List(
            [FromQuery] string? search,
            [FromQuery] string? role,
            [FromQuery] string? status,
            [FromQuery] bool? isActive,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var q = _db.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(u => u.Username.Contains(search) || u.FullName.Contains(search) ||
                                 (u.Email != null && u.Email.Contains(search)) ||
                                 (u.Phone != null && u.Phone.Contains(search)));
            if (!string.IsNullOrWhiteSpace(role))   q = q.Where(u => u.Role == role);
            if (!string.IsNullOrWhiteSpace(status)) q = q.Where(u => u.ApprovalStatus == status);
            if (isActive.HasValue)                  q = q.Where(u => u.IsActive == isActive.Value);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(u => u.CreatedAt)
                               .Skip((page-1)*pageSize).Take(pageSize)
                               .Select(u => new {
                                   u.Id, u.Username, u.FullName, u.Email, u.Phone, u.Role,
                                   u.AvatarUrl, u.ApprovalStatus, u.IsActive, u.CreatedAt, u.TeamId, u.LinkedPlayerId
                               }).ToListAsync();
            return Ok(new { items, page, pageSize, totalCount = total, totalPages = (int)Math.Ceiling(total / (double)pageSize) });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            return Ok(Sanitize(u));
        }

        public class CreateUserRequest
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string FullName { get; set; } = "";
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string Role { get; set; } = "Fan";
            public int? TeamId { get; set; }
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { message = "Username and password required." });
            if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                return Conflict(new { message = "Username already taken." });

            var user = new User
            {
                Username = req.Username.Trim(),
                Password = req.Password,
                FullName = req.FullName,
                Email = req.Email,
                Phone = req.Phone,
                Role = req.Role,
                TeamId = req.TeamId,
                ApprovalStatus = "Approved",
                ApprovedByUserId = User.GetUserId(),
                ApprovedAt = DateTime.UtcNow,
                IsActive = true
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Ok(Sanitize(user));
        }

        public class UpdateRoleRequest { public string Role { get; set; } = ""; }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id}/role")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleRequest req)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            u.Role = req.Role;
            await _db.SaveChangesAsync();
            return Ok(Sanitize(u));
        }

        public class StatusRequest { public bool IsActive { get; set; } }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] StatusRequest req)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            u.IsActive = req.IsActive;
            await _db.SaveChangesAsync();
            return Ok(Sanitize(u));
        }

        public class ResetPasswordRequest { public string NewPassword { get; set; } = ""; }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("{id}/reset-password")]
        public async Task<IActionResult> ResetPassword(int id, [FromBody] ResetPasswordRequest req)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            if (string.IsNullOrWhiteSpace(req.NewPassword) || req.NewPassword.Length < 6)
                return BadRequest(new { message = "Password must be at least 6 characters." });
            u.Password = req.NewPassword;
            await _db.SaveChangesAsync();
            return Ok(new { message = "Password reset." });
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var me = User.GetUserId();
            if (me == id) return BadRequest(new { message = "Cannot delete your own account." });
            var u = await _db.Users.FindAsync(id);
            if (u == null) return NotFound();
            _db.Users.Remove(u);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        private static object Sanitize(User u) => new
        {
            u.Id, u.Username, u.FullName, u.Email, u.Phone, u.Role,
            u.AvatarUrl, u.ApprovalStatus, u.IsActive, u.CreatedAt,
            u.TeamId, u.LinkedPlayerId
        };
    }
}
