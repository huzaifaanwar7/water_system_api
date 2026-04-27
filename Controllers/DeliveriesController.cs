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
    public class DeliveriesController : ControllerBase
    {
        private readonly GBS_DbContext _context;

        public DeliveriesController(GBS_DbContext context)
        {
            _context = context;
        }

        // GET: api/Deliveries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Delivery>>> GetDeliveries(int? customerId, string? status)
        {
            var query = _context.Deliveries.AsQueryable();

            if (customerId.HasValue)
            {
                query = query.Where(d => d.CustomerId == customerId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(d => d.PaymentStatus == status);
            }

            return await query.OrderByDescending(d => d.Date).ToListAsync();
        }

        // POST: api/Deliveries
        [HttpPost]
        public async Task<ActionResult<Delivery>> PostDelivery(Delivery delivery)
        {
            _context.Deliveries.Add(delivery);
            
            // Update customer balance and bottles out
            var customer = await _context.Customers.FindAsync(delivery.CustomerId);
            if (customer != null)
            {
                if (delivery.PaymentStatus == "pending" || delivery.PaymentStatus == "credit")
                {
                    customer.Balance -= delivery.TotalAmount;
                }
                customer.BottlesOut += (delivery.Bottles19L - delivery.Empty19L);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDelivery", new { id = delivery.Id }, delivery);
        }

        // GET: api/Deliveries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Delivery>> GetDelivery(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);

            if (delivery == null)
            {
                return NotFound();
            }

            return delivery;
        }

        // PUT: api/Deliveries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDelivery(int id, Delivery delivery)
        {
            if (id != delivery.Id)
            {
                return BadRequest();
            }

            _context.Entry(delivery).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id))
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

        // DELETE: api/Deliveries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDelivery(int id)
        {
            var delivery = await _context.Deliveries.FindAsync(id);
            if (delivery == null)
            {
                return NotFound();
            }

            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeliveryExists(int id)
        {
            return _context.Deliveries.Any(e => e.Id == id);
        }

        // POST: api/Deliveries/generate-invoice
        [HttpPost("generate-invoice")]
        public async Task<ActionResult<Invoice>> GenerateInvoice([FromBody] GenerateInvoiceRequest request)
        {
            if (request.DeliveryIds == null || !request.DeliveryIds.Any())
            {
                return BadRequest("No delivery IDs provided.");
            }

            var deliveries = await _context.Deliveries
                .Where(d => request.DeliveryIds.Contains(d.Id))
                .ToListAsync();

            if (!deliveries.Any())
            {
                return NotFound("No deliveries found for the provided IDs.");
            }

            var customerId = deliveries.First().CustomerId;
            if (deliveries.Any(d => d.CustomerId != customerId))
            {
                return BadRequest("All deliveries must belong to the same customer.");
            }

            var invoice = new Invoice
            {
                CustomerId = customerId,
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(7),
                TotalAmount = deliveries.Sum(d => d.TotalAmount),
                Status = "pending",
                Deliveries = deliveries
            };

            _context.Invoices.Add(invoice);
            
            // Link deliveries to the new invoice
            foreach (var delivery in deliveries)
            {
                delivery.Invoice = invoice;
            }

            await _context.SaveChangesAsync();

            return Ok(invoice);
        }

        public class GenerateInvoiceRequest
        {
            public List<int> DeliveryIds { get; set; } = new List<int>();
        }
    }
}
