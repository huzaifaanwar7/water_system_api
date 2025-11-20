using GBS.Data.Model;
using GBS.Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GBS.Service
{
    public class OrderService : IOrderService
    {
        private readonly GBS_DbContext _context;

        public OrderService(GBS_DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetOrderList()
        {
            return await _context.Orders
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetOrdersByClientId(int clientId)
        {
            return await _context.Orders
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .Where(o => o.ClientIdFk == clientId)
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
        }

        public async Task<string> GetUserNameById(int userId)
        {
            var user = await _context.Clients
                .Where(c => c.Id == userId)
                .Select(c => new { c.ClientName })
                .FirstOrDefaultAsync();

            return user?.ClientName ?? "Unknown User";
        }
    }
}