using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }

        public int? UserId { get; set; }

        [StringLength(100)]
        public string? SessionId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("UserId")]
        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public ICollection<CartItem>? CartItems { get; set; }

        [NotMapped]
        public decimal TotalAmount => CartItems?.Sum(i => i.Quantity * i.UnitPrice) ?? 0;

        [NotMapped]
        public int TotalItems => CartItems?.Sum(i => i.Quantity) ?? 0;
    }
}