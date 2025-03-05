using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Phone { get; set; }
        public string Role { get; set; } = "Customer";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // ✅ Chỉ giữ Id để tránh vòng lặp
        public ICollection<UserAddress>? UserAddresses { get; set; }

        [JsonIgnore] // 🚀 Tránh vòng lặp với Orders
        public ICollection<Order>? Orders { get; set; }

        [JsonIgnore] // 🚀 Tránh vòng lặp với ProductReviews
        public ICollection<ProductReview>? ProductReviews { get; set; }
    }
}
