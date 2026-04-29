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
    public class PaymentsController : ControllerBase
    {
        private readonly GBS_DbContext _context;

        public PaymentsController(GBS_DbContext context)
        {
            _context = context;
        }

        // GET: api/Payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments.Include(p => p.Customer).OrderByDescending(p => p.Date).ToListAsync();
        }

        // GET: api/Payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.Include(p => p.Customer).FirstOrDefaultAsync(p => p.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // POST: api/Payments
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payments.Add(payment);

            // Update customer balance
            var customer = await _context.Customers.FindAsync(payment.CustomerId);
            if (customer != null)
            {
                customer.Balance += payment.Amount;

                // Auto-allocate this general payment to the oldest unpaid deliveries
                if (payment.DeliveryId == null)
                {
                    decimal remainingPayment = payment.Amount;
                    
                    var unpaidDeliveries = await _context.Deliveries
                        .Where(d => d.CustomerId == payment.CustomerId && (d.PaymentStatus == "pending" || d.PaymentStatus == "credit" || d.PaymentStatus == "partial" || d.PaymentStatus == "udhaar"))
                        .OrderBy(d => d.Date)
                        .ToListAsync();

                    foreach (var delivery in unpaidDeliveries)
                    {
                        if (remainingPayment <= 0) break;

                        decimal amountDue = delivery.TotalAmount - delivery.AmountPaid;
                        if (amountDue <= 0) continue;

                        if (remainingPayment >= amountDue)
                        {
                            delivery.AmountPaid += amountDue;
                            delivery.PaymentStatus = "paid";
                            remainingPayment -= amountDue;
                        }
                        else
                        {
                            delivery.AmountPaid += remainingPayment;
                            delivery.PaymentStatus = "partial";
                            remainingPayment = 0;
                        }
                    }
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        // PUT: api/Payments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            var oldPayment = await _context.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (oldPayment == null) return NotFound();

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                var customer = await _context.Customers.FindAsync(payment.CustomerId);
                if (customer != null)
                {
                    // Revert old amount
                    customer.Balance -= oldPayment.Amount;
                    // Apply new amount
                    customer.Balance += payment.Amount;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // DELETE: api/Payments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            // Revert customer balance
            var customer = await _context.Customers.FindAsync(payment.CustomerId);
            if (customer != null)
            {
                customer.Balance -= payment.Amount;
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
