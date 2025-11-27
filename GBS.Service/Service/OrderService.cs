using Microsoft.EntityFrameworkCore;
using GBS.Entities.DbModels;
using GBS.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GBS.Service.Service  // Keep this namespace
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrderList();
        Task<Order> GetOrderById(int Id);
        Task<List<Order>> GetOrdersByClientId(int clientId);
    }

    public class OrderService : IOrderService
    {
        private readonly GBS_DbContext _dbContext;

        public OrderService(GBS_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Order>> GetOrderList()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .Include(o => o.OrderCosts)
                .Include(o => o.OrderItems)
                .ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderById(int Id)
        {
            return await _dbContext.Orders
                .Where(u => u.Id == Id)
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .Include(o => o.OrderCosts)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByClientId(int clientId)
        {
            return await _dbContext.Orders
                .Where(o => o.ClientIdFk == clientId)
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .Include(o => o.OrderCosts)
                .ToListAsync();
        }
    }
}