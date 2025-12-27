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

        public async Task<Payment> CreatePayment(PaymentVM model)
        {
            var payment = new Payment
            {
                OrderIdFk = model.OrderIdFk,
                PaymentDate = model.PaymentDate,
                Amount = model.Amount,
                PaymentMethodIdFk = model.PaymentMethodIdFk,
                ReferenceNumber = model.ReferenceNumber,
                Notes = model.Notes,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate != default ? model.CreatedDate : System.DateTime.Now
            };

            _dbContext.Payments.Add(payment);
            await _dbContext.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> UpdatePayment(int Id, PaymentVM model)
        {
            var payment = await _dbContext.Payments.FindAsync(Id);
            if (payment == null) return false;

            payment.OrderIdFk = model.OrderIdFk;
            payment.PaymentDate = model.PaymentDate;
            payment.Amount = model.Amount;
            payment.PaymentMethodIdFk = model.PaymentMethodIdFk;
            payment.ReferenceNumber = model.ReferenceNumber;
            payment.Notes = model.Notes;
            payment.CreatedBy = model.CreatedBy;

            _dbContext.Payments.Update(payment);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePayment(int Id)
        {
            var payment = await _dbContext.Payments.FindAsync(Id);
            if (payment == null) return false;

            _dbContext.Payments.Remove(payment);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
