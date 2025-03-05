using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int VariantId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [JsonIgnore] // 🚀 Tránh vòng lặp
        public Order Order { get; set; }

        public Product Product { get; set; }

        public ProductVariant ProductVariant { get; set; }
    }
}
