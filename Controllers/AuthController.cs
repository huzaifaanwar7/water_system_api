using GBS.Api.Authorization;
using GBS.Api.Data;
using GBS.Api.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBS.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly GBS_DbContext _db;
        private readonly IJwtUtils _jwt;

        public AuthController(GBS_DbContext db, IJwtUtils jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        public class RegisterRequest
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public string FullName { get; set; } = "";
            public string? Email { get; set; }
            public string? Phone { get; set; }
            // Captain | Scorer | Player | Fan  (SuperAdmin only via seed)
            public string Role { get; set; } = "Fan";
        }

        public class LoginRequest
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest(new { message = "Username and password required." });

            if (await _db.Users.AnyAsync(u => u.Username == req.Username))
                return Conflict(new { message = "Username already taken." });

            var role = req.Role ?? "Fan";
            // Self-registration is for fans only. Captain accounts are created and
            // assigned by SuperAdmin/Admin via the team-create flow.
            if (role == "SuperAdmin" || role == "Admin" || role == "Captain" || role == "Scorer")
                role = "Fan";
            var needsApproval = false;

            var user = new User
            {
                Username = req.Username.Trim(),
                Password = req.Password, // TODO: hash before prod
                FullName = req.FullName,
                Email = req.Email,
                Phone = req.Phone,
                Role = role,
                ApprovalStatus = needsApproval ? "Pending" : "Approved",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            if (needsApproval)
            {
                _db.ApprovalRequests.Add(new ApprovalRequest
                {
                    EntityType = "User",
                    EntityId = user.Id,
                    RequestedByUserId = user.Id,
                    Status = "Pending",
                    Notes = $"Captain signup: {user.FullName}"
                });
                await _db.SaveChangesAsync();
                return Ok(new { message = "Registered. Awaiting SuperAdmin approval.", userId = user.Id, approvalStatus = "Pending" });
            }

            var token = _jwt.GenerateJwtToken(user);
            return Ok(new { message = "Registered.", token, user = Sanitize(user) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username && u.Password == req.Password);
            if (user == null) return Unauthorized(new { message = "Invalid credentials." });
            if (!user.IsActive) return Unauthorized(new { message = "Account is suspended." });
            if (user.ApprovalStatus == "Pending") return Unauthorized(new { message = "Account pending SuperAdmin approval." });
            if (user.ApprovalStatus == "Rejected") return Unauthorized(new { message = "Account rejected." });

            var token = _jwt.GenerateJwtToken(user);
            return Ok(new { token, user = Sanitize(user) });
        }

        private static object Sanitize(User u) => new
        {
            u.Id, u.Username, u.FullName, u.Email, u.Phone, u.Role,
            u.AvatarUrl, u.ApprovalStatus, u.TeamId, u.LinkedPlayerId
        };
    }
}
