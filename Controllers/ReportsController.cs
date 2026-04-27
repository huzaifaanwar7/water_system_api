using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBS.Api.Data;
using Microsoft.AspNetCore.Authorization;

namespace GBS.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly GBS_DbContext _context;

        public ReportsController(GBS_DbContext context)
        {
            _context = context;
        }

        [HttpGet("daily-summary")]
        public async Task<IActionResult> GetDailySummary(DateTime? date)
        {
            var targetDate = (date ?? DateTime.Today).Date;
            
            var deliveries = await _context.Deliveries
                .Where(d => d.Date.Date == targetDate)
                .ToListAsync();

            var payments = await _context.Payments
                .Where(p => p.Date.Date == targetDate)
                .ToListAsync();

            var totalSales = deliveries.Sum(d => d.TotalAmount);
            var cashReceived = payments.Sum(p => p.Amount);
            var onCredit = deliveries.Where(d => d.PaymentStatus == "pending" || d.PaymentStatus == "credit").Sum(d => d.TotalAmount);

            var productQuantities = new
            {
                Bottles19L = deliveries.Sum(d => d.Bottles19L),
                Bottles15L = deliveries.Sum(d => d.Bottles15L),
                Bottles05L = deliveries.Sum(d => d.Bottles05L)
            };

            return Ok(new
            {
                Date = targetDate,
                TotalSales = totalSales,
                CashReceived = cashReceived,
                OnCredit = onCredit,
                DeliveryCount = deliveries.Count,
                ProductQuantities = productQuantities
            });
        }

        [HttpGet("udhaar-report")]
        public async Task<IActionResult> GetUdhaarReport()
        {
            var customers = await _context.Customers
                .Where(c => c.Balance < 0)
                .OrderBy(c => c.Balance)
                .Select(c => new
                {
                    c.Id,
                    c.Name,
                    c.Phone,
                    c.Balance,
                    c.BottlesOut
                })
                .ToListAsync();

            return Ok(customers);
        }

        [HttpGet("weekly-summary")]
        public async Task<IActionResult> GetWeeklySummary()
        {
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            var days = Enumerable.Range(0, 7).Select(i => startOfWeek.AddDays(i)).ToList();

            var summaries = new List<object>();

            foreach (var day in days)
            {
                var revenue = await _context.Deliveries
                    .Where(d => d.Date.Date == day.Date)
                    .SumAsync(d => d.TotalAmount);

                summaries.Add(new
                {
                    Label = day.ToString("ddd"),
                    Revenue = revenue,
                    Date = day.Date
                });
            }

            return Ok(summaries);
        }
    }
}
