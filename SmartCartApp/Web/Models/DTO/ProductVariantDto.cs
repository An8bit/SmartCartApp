namespace Web.Models.DTO
{
    public class ProductVariantDto
    {
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal? AdditionalPrice { get; set; }
        public int StockQuantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
