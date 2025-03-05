using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int UserId { get; set; }  // Giữ khóa ngoại UserId
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = "Pending";
        public int ShippingAddressId { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore] 
        public User? User { get; set; }

        public UserAddress? ShippingAddress { get; set; }

        [JsonIgnore] 
        public ICollection<OrderItem>? OrderItems { get; set; }

        [JsonIgnore] 
        public ICollection<OrderHistory>? OrderHistories { get; set; }
    }
}
