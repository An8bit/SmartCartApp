namespace Web.Models.DTO
{
    public class ProductReviewDto
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; } // 1 - 5 sao
        public string ReviewText { get; set; }
        public string Response { get; set; } // Phản hồi từ Admin
        public DateTime CreatedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
    }
}
