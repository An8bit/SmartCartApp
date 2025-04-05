using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO.PaymentDTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(100)]
        public string? TransactionId { get; set; }
    }

    public class CreatePaymentDTO
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; } // "MoMo" hoặc "Banking"

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        // Thông tin bổ sung cho thanh toán
        public Dictionary<string, string> PaymentDetails { get; set; }
    }

    public class UpdatePaymentDTO
    {
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(100)]
        public string? TransactionId { get; set; }
    }
}