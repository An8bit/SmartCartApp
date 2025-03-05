using System.ComponentModel.DataAnnotations;

namespace Web.Models.Domain
{
    public class ProductVariant
    {
        [Key]
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int StockQuantity { get; set; }

        public Product Product { get; set; }
    }
}
