using Web.Stratery;

namespace SmartCartApp.Models.Payment
{
    public class PaymentContext
    {
        private IPaymentStrategy _paymentStrategy;

        public PaymentContext()
        {
        }

        public PaymentContext(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
        {
            _paymentStrategy = paymentStrategy;
        }

        public bool ProcessPayment(decimal amount)
        {
            if (_paymentStrategy == null)
            {
                throw new InvalidOperationException("Chưa thiết lập phương thức thanh toán");
            }

            return _paymentStrategy.Pay(amount);
        }

        public string GetPaymentMethod()
        {
            if (_paymentStrategy == null)
            {
                throw new InvalidOperationException("Chưa thiết lập phương thức thanh toán");
            }

            return _paymentStrategy.GetPaymentMethod();
        }
    }
}