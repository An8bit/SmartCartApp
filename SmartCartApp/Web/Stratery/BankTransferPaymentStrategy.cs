using Web.Models.Domain;

namespace Web.Stratery
{
    public class BankTransferPaymentStrategy : IPaymentStrategy
    {
        public string Name => "Bank Transfer";

        private readonly string _bankAccount;
        private readonly string _bankName;
        private readonly string _accountHolder;

        public BankTransferPaymentStrategy(string bankAccount, string bankName, string accountHolder)
        {
            _bankAccount = bankAccount;
            _bankName = bankName;
            _accountHolder = accountHolder;
        }

        public bool ProcessPayment(Order order, decimal amount)
        {
            
            Console.WriteLine($"Processing bank transfer for order {order.OrderId} with amount {amount:C}");
            Console.WriteLine($"Transfer to account: {_bankAccount}, Bank: {_bankName}, Account holder: {_accountHolder}");

            // Simulate successful payment
            return true;
        }

        public string GetPaymentInformation()
        {
            return $"Please transfer to the following bank account:\nAccount Number: {_bankAccount}\nBank: {_bankName}\nAccount Holder: {_accountHolder}";
        }
    }
}