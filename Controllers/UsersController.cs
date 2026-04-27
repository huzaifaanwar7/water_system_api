using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBS.Api.Data;
using GBS.Api.DbModels;
using GBS.Api.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace GBS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]

    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GBS_DbContext _context;
        private readonly IJwtUtils _jwtUtils;

        public UsersController(GBS_DbContext context, IJwtUtils jwtUtils)
        {
            _context = context;
            _jwtUtils = jwtUtils;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Users/login
        [AllowAnonymous]
        [HttpPost("login")]

        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _jwtUtils.GenerateJwtToken(user);

            return Ok(new LoginResponse
            {
                User = user,
                Token = token
            });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public class LoginRequest
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public User User { get; set; } = null!;
            public string Token { get; set; } = string.Empty;
        }
    }
}
