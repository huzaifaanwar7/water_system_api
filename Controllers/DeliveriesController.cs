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

            return await query.Include(d => d.Customer).OrderByDescending(d => d.Date).ToListAsync();
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
                var status = (delivery.PaymentStatus ?? "").ToLower();
                if (status == "pending" || status == "credit")
                {
                    customer.Balance -= delivery.TotalAmount;
                }
                else
                {
                    // It's a paid delivery (cash, easypaisa, jazzcash)
                    // We should record it as a payment so it shows up in "Total Received"
                    var payment = new Payment
                    {
                        Date = delivery.Date,
                        CustomerId = delivery.CustomerId,
                        Amount = delivery.TotalAmount,
                        Method = delivery.PaymentStatus, // "cash", "easypaisa", or "jazzcash"
                        Notes = "Auto-generated from delivery #" + delivery.Id
                    };
                    _context.Payments.Add(payment);
                }
                customer.BottlesOut += (delivery.Bottles19L - delivery.Empty19L);
            }

            await UpdateInventory(delivery, false);

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

            var oldDelivery = await _context.Deliveries.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
            if (oldDelivery == null) return NotFound();

            _context.Entry(delivery).State = EntityState.Modified;

            try
            {
                // Revert old balance/bottles
                var customer = await _context.Customers.FindAsync(delivery.CustomerId);
                if (customer != null)
                {
                    // Revert old
                    var oldStatus = (oldDelivery.PaymentStatus ?? "").ToLower();
                    if (oldStatus == "pending" || oldStatus == "credit")
                        customer.Balance += oldDelivery.TotalAmount;
                    customer.BottlesOut -= (oldDelivery.Bottles19L - oldDelivery.Empty19L);

                    // Apply new
                    var newStatus = (delivery.PaymentStatus ?? "").ToLower();
                    if (newStatus == "pending" || newStatus == "credit")
                        customer.Balance -= delivery.TotalAmount;
                    customer.BottlesOut += (delivery.Bottles19L - delivery.Empty19L);

                    // Update Inventory
                    await UpdateInventory(oldDelivery, true); // Revert old
                    await UpdateInventory(delivery, false);   // Apply new
                }

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

            // Revert customer balance and bottles out
            var customer = await _context.Customers.FindAsync(delivery.CustomerId);
            if (customer != null)
            {
                if (delivery.PaymentStatus == "pending" || delivery.PaymentStatus == "credit")
                {
                    customer.Balance += delivery.TotalAmount;
                }
                customer.BottlesOut -= (delivery.Bottles19L - delivery.Empty19L);
            }

            await UpdateInventory(delivery, true);

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

        private async Task UpdateInventory(Delivery delivery, bool isReverting)
        {
            int factor = isReverting ? 1 : -1;
            var configs = await _context.BottleConfigs.ToListAsync();

            var c19 = configs.FirstOrDefault(c => c.Name == "19L");
            if (c19 != null) c19.CurrentStock += factor * (delivery.Bottles19L - delivery.Empty19L);

            var c15 = configs.FirstOrDefault(c => c.Name == "1.5L");
            if (c15 != null) c15.CurrentStock += factor * (delivery.Bottles15L - delivery.Empty15L);

            var c05 = configs.FirstOrDefault(c => c.Name == "0.5L");
            if (c05 != null) c05.CurrentStock += factor * (delivery.Bottles05L - delivery.Empty05L);
        }
    }
}
