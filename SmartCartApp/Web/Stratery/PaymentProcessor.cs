using Web.Models.Domain;

namespace Web.Stratery
{
    public class PaymentProcessor
    {
        private readonly Dictionary<string, IPaymentStrategy> _availablePaymentStrategies;

        public PaymentProcessor(IEnumerable<IPaymentStrategy> paymentStrategies)
        {
            _availablePaymentStrategies = paymentStrategies.ToDictionary(p => p.Name, p => p);
        }

        public IEnumerable<string> GetAvailablePaymentMethods()
        {
            return _availablePaymentStrategies.Keys;
        }

        public bool ProcessPayment(string paymentMethod, Order order, decimal amount)
        {
            if (!_availablePaymentStrategies.TryGetValue(paymentMethod, out var paymentStrategy))
            {
                throw new KeyNotFoundException($"Payment method '{paymentMethod}' not found.");
            }

            return paymentStrategy.ProcessPayment(order, amount);
        }

        public string GetPaymentInformation(string paymentMethod)
        {
            if (!_availablePaymentStrategies.TryGetValue(paymentMethod, out var paymentStrategy))
            {
                throw new KeyNotFoundException($"Payment method '{paymentMethod}' not found.");
            }

            return paymentStrategy.GetPaymentInformation();
        }
    }
}