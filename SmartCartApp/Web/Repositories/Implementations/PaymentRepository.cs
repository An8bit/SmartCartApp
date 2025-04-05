using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models.Domain;
using Web.Repositories.Contracts;

namespace Web.Repositories.Implementations
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public  async Task<Payment> AddPaymentAsync(Payment payment)
        {
           var result = await _context.Payments.AddAsync(payment);
            return payment;
        }

        public async Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByPaymentMethodAsync(string paymentMethod)
        {
            return await _context.Payments
                .Where(p => p.PaymentMethod == paymentMethod)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<int> GetCountByPaymentMethodAsync(string paymentMethod)
        {
            return await _context.Payments
                .CountAsync(p => p.PaymentMethod == paymentMethod);
        }

        public async Task<decimal> GetTotalAmountByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                .SumAsync(p => p.Amount);
        }

       

        public async Task UpdateStatusAsync(int id, string status, string failureReason = null)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment != null)
            {
                payment.Status = status;
                payment.FailureReason = failureReason;
                _context.Payments.Update(payment);
                await _context.SaveChangesAsync();
            }
        }

        async Task<bool> IPaymentRepository.DeleteAsync(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null) return false;
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
            return true;
        }

        async Task<IEnumerable<Payment>> IPaymentRepository.GetAllAsync()
        {
           var payments = await _context.Payments.ToListAsync();
            return payments;
        }

        async Task<Payment> IPaymentRepository.GetByIdAsync(int id)
        {
            return await _context.Payments.FindAsync(id);
        }
    }
}
