using Web.Models.Domain;
using Web.Models.DTO.PaymentDTOs;

namespace Web.Repositories.Interfaces.IServices
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(CreatePaymentDTO paymentDTO);

        Task<IEnumerable<Payment>> GetPaymentHistoryAsync();

        Task<Payment> GetPaymentByIdAsync(int id);

        Task<IEnumerable<Payment>> GetPaymentsByOrderIdAsync(int orderId);
    }
}