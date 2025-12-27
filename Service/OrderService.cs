using Microsoft.EntityFrameworkCore;
using GBS.Api.DbModels;
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
        Task<int> SaveOrder(Order order);
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
                .Include(o => o.OrderLabors)
                .Include(o => o.OrderMaterials)
                .Include(o => o.OrderStatusHistories)
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
        public async Task<int> SaveOrder(Order order)
        {
            if (order.Id == 0) { await _dbContext.Orders.AddAsync(order); }
            else { _dbContext.Orders.Update(order); }
            return await _dbContext.SaveChangesAsync();
        }
    }
}