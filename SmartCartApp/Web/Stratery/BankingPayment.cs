using Web.Models.Domain;

namespace Web.Stratery
{
    public class BankingPayment : IPaymentStrategy
    {
        private readonly string _bankName;
        private readonly string _accountNumber;

        public BankingPayment(string bankName, string accountNumber)
        {
            _bankName = bankName;
            _accountNumber = accountNumber;
        }

        public bool Pay(decimal amount)
        {
            // Xử lý logic thanh toán qua Banking
            Console.WriteLine($"Thanh toán {amount} qua ngân hàng {_bankName} với số tài khoản {_accountNumber}");
            return true;
        }

        public string GetPaymentMethod()
        {
            return "Banking";
        }
    }
}
    
