using Microsoft.EntityFrameworkCore;
using GBS.Api.DbModels;
using GBS.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GBS.Service.Service  // Keep this namespace
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetPaymentList();
        Task<Payment> GetPaymentById(int Id);
        Task<List<Payment>> GetPaymentsByOrderId(int orderId);
        Task<int> SavePayment(Payment orderId);
        Task<decimal> GetTotalPaymentsByOrderId(int orderId);

    }

    public class PaymentService : IPaymentService
    {
        private readonly GBS_DbContext _dbContext;

        public PaymentService(GBS_DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Payment>> GetPaymentList()
        {
            return await _dbContext.Payments
                .Include(p => p.OrderIdFkNavigation) // Optional navigation if needed
                .Include(p => p.PaymentMethodIdFkNavigation) // Optional navigation
                .ToListAsync();
        }

        public async Task<Payment> GetPaymentById(int Id)
        {
            return await _dbContext.Payments
                .Where(p => p.Id == Id)
                .Include(p => p.OrderIdFkNavigation)
                .Include(p => p.PaymentMethodIdFkNavigation)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Payment>> GetPaymentsByOrderId(int orderId)
        {
            return await _dbContext.Payments
                .Where(p => p.OrderIdFk == orderId)
                .Include(p => p.OrderIdFkNavigation)
                .Include(p => p.PaymentMethodIdFkNavigation)
                .ToListAsync();
        }

        public async Task<int> SavePayment(Payment payment)
        {
            if (payment.Id == 0)
            {
                await _dbContext.Payments.AddAsync(payment);
            }
            else
            {
                _dbContext.Payments.Update(payment);
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeletePayment(int Id)
        {
            var payment = await _dbContext.Payments.FindAsync(Id);
            if (payment == null) return false;

            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<decimal> GetTotalPaymentsByOrderId(int orderId)
        {
            return await _dbContext.Payments
                .Where(p => p.OrderIdFk == orderId)
                .SumAsync(p => p.Amount.Value);
        }
    }
}
