using Web.Models.Domain;

namespace Web.Stratery
{
    public interface IPaymentStrategy
    {
        string Name { get; }
        bool ProcessPayment(Order order, decimal amount);
        string GetPaymentInformation();
    }
}
