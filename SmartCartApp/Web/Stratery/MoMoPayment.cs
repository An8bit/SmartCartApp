using Web.Models.Domain;

namespace Web.Stratery
{
    public class MoMoPayment : IPaymentStrategy
    {
        private readonly string _phoneNumber;
        private readonly string _otp;

        public MoMoPayment(string phoneNumber, string otp)
        {
            _phoneNumber = phoneNumber;
            _otp = otp;
        }

        public bool Pay(decimal amount)
        {
            // Xử lý logic thanh toán qua MoMo
            Console.WriteLine($"Thanh toán {amount} qua ví MoMo với số điện thoại {_phoneNumber}");
            return true;
        }

        public string GetPaymentMethod()
        {
            return "MoMo";
        }
    }
}