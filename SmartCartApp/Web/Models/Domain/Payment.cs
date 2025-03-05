using System.ComponentModel.DataAnnotations;

namespace Web.Models.Domain
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        public string TransactionId { get; set; }  // Mã giao dịch từ cổng thanh toán

        public Order Order { get; set; }
    }
}
