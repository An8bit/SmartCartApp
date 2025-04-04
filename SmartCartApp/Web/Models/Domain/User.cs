using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Web.UserStates;

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
        // Thêm các thuộc tính phân hạng thành viên
        [Required]
        [StringLength(3)]
        public string MembershipTier { get; set; } = "STD";
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalSpending { get; set; } = 0;

        public DateTime LastPurchaseDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // ✅ Chỉ giữ Id để tránh vòng lặp
        public ICollection<UserAddress>? UserAddresses { get; set; }

        [JsonIgnore] // 🚀 Tránh vòng lặp với Orders
        public ICollection<Order>? Orders { get; set; }

        [JsonIgnore] // 🚀 Tránh vòng lặp với ProductReviews
        public ICollection<ProductReview>? ProductReviews { get; set; }

        // Thuộc tính không ánh xạ để truy cập state
        [NotMapped]
        [JsonIgnore]
        private IUserMembershipState? _state;

        [NotMapped]
        public IUserMembershipState State
        {
            get
            {
                if (_state == null)
                {
                    var context = new UserStateContext();
                    _state = context.GetState(MembershipTier);
                }
                return _state;
            }
        }

        // Method để tính toán giá sản phẩm cho user này
        public decimal GetDiscountedPrice(decimal originalPrice)
        {
            return State.CalculateDiscountedPrice(originalPrice);
        }
    }
}