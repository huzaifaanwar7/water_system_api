using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBS.Api.Data;
using GBS.Api.DbModels;
using Microsoft.AspNetCore.Authorization;

namespace GBS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BottleConfigsController : ControllerBase
    {
        private readonly GBS_DbContext _context;

        public BottleConfigsController(GBS_DbContext context)
        {
            _context = context;
        }

        // GET: api/BottleConfigs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BottleConfig>>> GetBottleConfigs()
        {
            return await _context.BottleConfigs.ToListAsync();
        }

        // GET: api/BottleConfigs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BottleConfig>> GetBottleConfig(int id)
        {
            var bottleConfig = await _context.BottleConfigs.FindAsync(id);

            if (bottleConfig == null)
            {
                return NotFound();
            }

            return bottleConfig;
        }

        // POST: api/BottleConfigs
        [HttpPost]
        public async Task<ActionResult<BottleConfig>> PostBottleConfig(BottleConfig bottleConfig)
        {
            _context.BottleConfigs.Add(bottleConfig);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBottleConfig", new { id = bottleConfig.Id }, bottleConfig);
        }

        // PUT: api/BottleConfigs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBottleConfig(int id, BottleConfig bottleConfig)
        {
            if (id != bottleConfig.Id)
            {
                return BadRequest();
            }

            _context.Entry(bottleConfig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BottleConfigExists(id))
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

        // DELETE: api/BottleConfigs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBottleConfig(int id)
        {
            var bottleConfig = await _context.BottleConfigs.FindAsync(id);
            if (bottleConfig == null)
            {
                return NotFound();
            }

            _context.BottleConfigs.Remove(bottleConfig);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BottleConfigExists(int id)
        {
            return _context.BottleConfigs.Any(e => e.Id == id);
        }
    }
}
