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
            var status = (delivery.PaymentStatus ?? "").ToLower();

            // Handle AmountReceived for partial payments
            delivery.AmountPaid = delivery.AmountReceived;
            if (status == "pending" || status == "credit" || status == "udhaar")
            {
                if (delivery.AmountPaid >= delivery.TotalAmount)
                {
                    delivery.PaymentStatus = "paid";
                    status = "paid";
                }
                else if (delivery.AmountPaid > 0)
                {
                    delivery.PaymentStatus = "partial";
                    status = "partial";
                }
            }
            else
            {
                // If paid fully in cash/bank/jazzcash directly
                delivery.AmountPaid = delivery.TotalAmount;
            }

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync(); // Save to generate delivery.Id

            // Update customer balance and bottles out
            var customer = await _context.Customers.FindAsync(delivery.CustomerId);
            if (customer != null)
            {
                customer.Balance -= delivery.TotalAmount; // Charge full amount

                if (delivery.AmountPaid > 0)
                {
                    var payment = new Payment
                    {
                        Date = delivery.Date,
                        CustomerId = delivery.CustomerId,
                        Amount = delivery.AmountPaid,
                        Method = (status == "pending" || status == "credit" || status == "partial") ? "cash" : delivery.PaymentStatus,
                        Notes = "Auto-generated payment for delivery #" + delivery.Id,
                        DeliveryId = delivery.Id
                    };
                    _context.Payments.Add(payment);
                    customer.Balance += delivery.AmountPaid; // Credit the paid amount
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
        public async Task<IActionResult> PutDelivery(int id, [FromBody] Delivery delivery)
        {
            var existing = await _context.Deliveries.Include(d => d.Customer).FirstOrDefaultAsync(d => d.Id == id);
            if (existing == null) return NotFound();

            var oldStatus = (existing.PaymentStatus ?? "").ToLower();
            var newStatus = (delivery.PaymentStatus ?? "").ToLower();

            // Revert old impact
            var customer = await _context.Customers.FindAsync(existing.CustomerId);
            if (customer != null)
            {
                customer.Balance += existing.TotalAmount;
                customer.Balance -= existing.AmountPaid; // Revert the payment credit
                customer.BottlesOut -= (existing.Bottles19L - existing.Empty19L);
            }
            await UpdateInventory(existing, true);

            // Calculate new AmountPaid based on new status if it was changed
            if (newStatus != oldStatus && (newStatus == "paid" || newStatus == "cash" || newStatus == "bank" || newStatus == "jazzcash"))
            {
                delivery.AmountPaid = delivery.TotalAmount;
            }

            // Update fields
            existing.Date = delivery.Date;
            existing.Bottles19L = delivery.Bottles19L;
            existing.Bottles15L = delivery.Bottles15L;
            existing.Bottles05L = delivery.Bottles05L;
            existing.Empty19L = delivery.Empty19L;
            existing.Empty15L = delivery.Empty15L;
            existing.Empty05L = delivery.Empty05L;
            existing.TotalAmount = delivery.TotalAmount;
            existing.PaymentStatus = delivery.PaymentStatus;
            existing.Notes = delivery.Notes;
            existing.AmountPaid = delivery.AmountPaid;

            // Apply new impact
            if (customer != null)
            {
                customer.Balance -= existing.TotalAmount;
                if (existing.AmountPaid > 0)
                {
                    customer.Balance += existing.AmountPaid;
                }
                customer.BottlesOut += (existing.Bottles19L - existing.Empty19L);
                
                var linkedPayment = await _context.Payments.FirstOrDefaultAsync(p => p.DeliveryId == existing.Id);
                if (linkedPayment != null)
                {
                    linkedPayment.Amount = existing.AmountPaid;
                    linkedPayment.Method = newStatus == "pending" ? "cash" : newStatus;
                }
                else if (existing.AmountPaid > 0)
                {
                    _context.Payments.Add(new Payment
                    {
                        Date = DateTime.Now,
                        CustomerId = existing.CustomerId,
                        Amount = existing.AmountPaid,
                        Method = newStatus == "pending" ? "cash" : newStatus,
                        Notes = "Payment for delivery #" + existing.Id,
                        DeliveryId = existing.Id,
                        InvoiceId = existing.InvoiceId
                    });
                }
            }
            await UpdateInventory(existing, false);

            // Sync Invoice Status if linked
            if (existing.InvoiceId.HasValue)
            {
                var invoice = await _context.Invoices
                    .Include(i => i.Deliveries)
                    .FirstOrDefaultAsync(i => i.Id == existing.InvoiceId.Value);

                if (invoice != null)
                {
                    bool allPaid = invoice.Deliveries.All(d => {
                        var s = (d.PaymentStatus ?? "").ToLower();
                        return s != "pending" && s != "credit" && s != "partial";
                    });
                    invoice.Status = allPaid ? "paid" : "pending";
                    invoice.AmountPaid = invoice.Deliveries.Sum(d => d.AmountPaid);
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id)) return NotFound();
                else throw;
            }

            return Ok(existing);
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
                customer.Balance += delivery.TotalAmount; // Revert the charge
                customer.Balance -= delivery.AmountPaid; // Revert the payment that will be cascade deleted
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
                AmountPaid = deliveries.Sum(d => d.AmountPaid),
                Status = deliveries.All(d => {
                    var s = (d.PaymentStatus ?? "").ToLower();
                    return s != "pending" && s != "credit" && s != "partial";
                }) ? "paid" : "pending",
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
