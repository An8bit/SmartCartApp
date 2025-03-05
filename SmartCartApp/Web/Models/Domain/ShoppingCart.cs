using System.ComponentModel.DataAnnotations;

namespace Web.Models.Domain
{
    public class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Product Product { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
