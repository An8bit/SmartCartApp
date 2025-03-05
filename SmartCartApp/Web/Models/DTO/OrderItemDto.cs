namespace Web.Models.DTO
{
    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } // Lấy từ Product
        public int VariantId { get; set; }
        public string VariantName { get; set; } // Lấy từ ProductVariant
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
