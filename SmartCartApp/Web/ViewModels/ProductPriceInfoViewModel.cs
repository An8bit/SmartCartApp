namespace Web.ViewModels
{
    public class ProductPriceInfoViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public bool IsOnSale { get; set; }
    }
}
