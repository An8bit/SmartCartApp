using Web.Models.Domain;

namespace Web.Stratery
{
    public class MoMoPaymentStrategy : IPaymentStrategy
    {
        public string Name => "MOMO";

        private readonly string _apiKey;
        private readonly string _partnerCode;

        public MoMoPaymentStrategy(string apiKey, string partnerCode)
        {
            _apiKey = apiKey;
            _partnerCode = partnerCode;
        }

        public bool ProcessPayment(Order order, decimal amount)
        {
            
            Console.WriteLine($"Processing MOMO payment for order {order.OrderId} with amount {amount:C}");
            Console.WriteLine($"Using partner code: {_partnerCode}");

            // Simulate successful payment
            return true;
        }

        public string GetPaymentInformation()
        {
            return "Scan the MOMO QR code or enter your MOMO account details to complete the payment.";
        }
    }
}