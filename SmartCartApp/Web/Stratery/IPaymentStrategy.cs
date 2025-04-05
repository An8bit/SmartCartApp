using Web.Models.Domain;

namespace Web.Stratery
{
    public interface IPaymentStrategy
    {
        bool Pay(decimal amount);
        string GetPaymentMethod();
    }
}
