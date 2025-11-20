using GBS.Entities.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GBS.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrderList();
        Task<Order> GetOrderById(int orderId);
        Task<IEnumerable<Order>> GetOrdersByClientId(int clientId);
        Task<string> GetUserNameById(int userId);
    }
}