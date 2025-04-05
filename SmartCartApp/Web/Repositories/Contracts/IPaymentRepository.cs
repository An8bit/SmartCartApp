using Web.Models.Domain;

namespace Web.Repositories.Contracts
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);

        Task<IEnumerable<Payment>> GetAllAsync();

        Task<IEnumerable<Payment>> GetByStatusAsync(string status);

        Task<IEnumerable<Payment>> GetByPaymentMethodAsync(string paymentMethod);

        Task<IEnumerable<Payment>> GetByOrderIdAsync(int orderId);

        Task<IEnumerable<Payment>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<Payment> AddPaymentAsync(Payment payment);

        Task UpdateAsync(Payment payment);

        Task UpdateStatusAsync(int id, string status, string failureReason = null);

        Task<bool> DeleteAsync(int id);

        Task<decimal> GetTotalAmountByDateRangeAsync(DateTime startDate, DateTime endDate);

        Task<int> GetCountByPaymentMethodAsync(string paymentMethod);
    }
}