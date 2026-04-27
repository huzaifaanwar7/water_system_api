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
    public class InvoicesController : ControllerBase
    {
        private readonly GBS_DbContext _context;

        public InvoicesController(GBS_DbContext context)
        {
            _context = context;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.Include(i => i.Customer).ToListAsync();
        }

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Deliveries)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }

        // GET: api/Invoices/customer/5
        [HttpGet("customer/{customerId}")]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetCustomerInvoices(int customerId)
        {
            return await _context.Invoices
                .Where(i => i.CustomerId == customerId)
                .OrderByDescending(i => i.InvoiceDate)
                .ToListAsync();
        }

        // POST: api/Invoices
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        // PUT: api/Invoices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
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

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Invoices/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            invoice.Status = status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Invoices/5/pay
        [HttpPost("{id}/pay")]
        public async Task<IActionResult> PayInvoice(int id, [FromBody] string method)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Deliveries)
                .Include(i => i.Customer)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null) return NotFound();
            if (invoice.Status == "paid") return BadRequest("Invoice is already paid.");

            decimal totalToPay = 0;
            foreach (var delivery in invoice.Deliveries)
            {
                var status = (delivery.PaymentStatus ?? "").ToLower();
                if (status == "pending" || status == "credit")
                {
                    totalToPay += delivery.TotalAmount;
                    delivery.PaymentStatus = method;
                    
                    // Update customer balance for this delivery
                    if (invoice.Customer != null)
                    {
                        invoice.Customer.Balance += delivery.TotalAmount;
                    }
                }
            }

            if (totalToPay > 0)
            {
                // Create a single payment record for the invoice
                var payment = new Payment
                {
                    Date = DateTime.Now,
                    CustomerId = invoice.CustomerId,
                    Amount = totalToPay,
                    Method = method,
                    Notes = "Paid Invoice #" + invoice.Id,
                    InvoiceId = invoice.Id
                };
                _context.Payments.Add(payment);
            }

            invoice.Status = "paid";
            await _context.SaveChangesAsync();

            return Ok(invoice);
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
