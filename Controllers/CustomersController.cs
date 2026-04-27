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
    public class CustomersController : ControllerBase
    {
        private readonly GBS_DbContext _context;

        public CustomersController(GBS_DbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // GET: api/Customers/5/deliveries
        [HttpGet("{id}/deliveries")]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetCustomerDeliveries(int id)
        {
            var deliveries = await _context.Deliveries
                .Where(d => d.CustomerId == id)
                .OrderByDescending(d => d.Date)
                .ToListAsync();

            return deliveries;
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers
                .Include(c => c.Deliveries)
                .Include(c => c.Payments)
                .Include(c => c.Invoices)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            // Remove related records first
            if (customer.Deliveries != null && customer.Deliveries.Any())
            {
                _context.Deliveries.RemoveRange(customer.Deliveries);
            }

            if (customer.Payments != null && customer.Payments.Any())
            {
                _context.Payments.RemoveRange(customer.Payments);
            }

            if (customer.Invoices != null && customer.Invoices.Any())
            {
                _context.Invoices.RemoveRange(customer.Invoices);
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
