using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        [JsonIgnore] // 
        public Category Category { get; set; }

        public ICollection<ProductVariant> Variants { get; set; }

        [JsonIgnore] // 
        public ICollection<ProductReview> Reviews { get; set; }

        [JsonIgnore]
        public virtual ICollection<Discount> Discounts { get; set; }
    }
}
