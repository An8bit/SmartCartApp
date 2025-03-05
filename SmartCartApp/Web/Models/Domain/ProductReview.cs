using System.ComponentModel.DataAnnotations;

namespace Web.Models.Domain
{
    public class ProductReview
    {
        [Key]
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; } // 1 - 5 sao
        public string ReviewText { get; set; }
        public string Response { get; set; } // Phản hồi từ Admin
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? RespondedAt { get; set; }

        public Product Product { get; set; }
        public User User { get; set; }
    }
}
