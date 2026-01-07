using Microsoft.EntityFrameworkCore;
using GBS.Api.DbModels;
using GBS.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GBS.Service.Service  // Keep this namespace
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrderList();
        Task<Order> GetOrderById(int Id);
        Task<List<Order>> GetOrdersByClientId(int clientId);
        Task<int> SaveOrder(Order order);
        Task<int> SaveOrderCost(OrderCost cost);
        Task<int> SaveOrderItem(OrderItem item);
        Task<int> SaveOrderLabor(OrderLabor labor);
        Task<int> SaveOrderMaterial(OrderMaterial material);
        Task<bool> SaveOrderStatusHistory(OrderStatusHistory statusHistory);

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
                .ThenInclude(o => o.ProductIdFkNavigation)
                 .Include(o => o.OrderItems)
                 .ThenInclude(o => o.SizeIdFkNavigation)
                .Include(o => o.OrderLabors)
                .Include(o => o.OrderMaterials)
                .Include(o => o.OrderStatusHistories)
                .ToListAsync();
            return orders;
        }

        public async Task<Order> GetOrderById(int Id)
        {
            return await _dbContext.Orders
                .Where(o => o.Id == Id)
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .Include(o => o.OrderCosts)
                .Include(o => o.OrderItems)
                .ThenInclude(o => o.ProductIdFkNavigation)
                 .Include(o => o.OrderItems)
                 .ThenInclude(o => o.SizeIdFkNavigation)
                .Include(o => o.OrderLabors)
                .Include(o => o.OrderMaterials)
                .Include(o => o.OrderStatusHistories)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByClientId(int clientId)
        {
            return await _dbContext.Orders
                .Where(o => o.ClientIdFk == clientId)
                .Include(o => o.ClientIdFkNavigation)
                .Include(o => o.StatusIdFkNavigation)
                .Include(o => o.OrderCosts)
                .Include(o => o.OrderItems)
                .Include(o => o.OrderLabors)
                .Include(o => o.OrderMaterials)
                .Include(o => o.OrderStatusHistories)
                .ToListAsync();
        }

        public async Task<int> SaveOrder(Order order)
        {
            if (order.Id == 0) { await _dbContext.Orders.AddAsync(order); }
            else { _dbContext.Orders.Update(order); }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> SaveOrderCost(OrderCost cost)
        {
            if (cost.Id == 0) { await _dbContext.OrderCosts.AddAsync(cost); }
            else { _dbContext.OrderCosts.Update(cost); }
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> SaveOrderItem(OrderItem item)
        {
            if (item.Id == 0)
            {
                await _dbContext.OrderItems.AddAsync(item);
            }
            else
            {
                _dbContext.OrderItems.Update(item);
            }

            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> SaveOrderLabor(OrderLabor labor)
        {
            if (labor.Id == 0) { await _dbContext.OrderLabors.AddAsync(labor); }
            else { _dbContext.OrderLabors.Update(labor); }
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> SaveOrderMaterial(OrderMaterial material)
        {
            if (material.Id == 0) { await _dbContext.OrderMaterials.AddAsync(material); }
            else { _dbContext.OrderMaterials.Update(material); }
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<bool> SaveOrderStatusHistory(OrderStatusHistory statusHistory)
        {
            if (statusHistory.Id == 0) { await _dbContext.OrderStatusHistories.AddAsync(statusHistory); }
            else { _dbContext.OrderStatusHistories.Update(statusHistory); }
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
