using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO.ProductDTOs
{
    // Product DTOs
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public List<ProductVariantDto> Variants { get; set; } = new List<ProductVariantDto>();

        public double? AverageRating { get; set; }

        public int ReviewsCount { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public string? MembershipTierName { get; set; }
    }

    public class ProductVariantDto
    {
        public int VariantId { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

       

        public int StockQuantity { get; set; }

       
    }

    public class ProductReviewSummaryDto
    {
        public double AverageRating { get; set; }

        public int ReviewsCount { get; set; }
    }

    // Trong CreateProductDto
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải từ 3 đến 100 ký tự")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "Mô tả không được vượt quá 2000 ký tự")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc")]
        [Range(0.01, 1000000000, ErrorMessage = "Giá phải lớn hơn 0 và nhỏ hơn 1 tỷ")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Danh mục sản phẩm là bắt buộc")]
        public int CategoryId { get; set; }

        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        public string ImageUrl { get; set; }

        // Biến thể không cần ProductId vì nó sẽ được gán tự động
        public List<CreateProductVariantInlineDto> Variants { get; set; } = new List<CreateProductVariantInlineDto>();
    }

    // DTO đặc biệt cho biến thể khi tạo sản phẩm mới
    public class CreateProductVariantInlineDto
    {
        [MaxLength(50, ErrorMessage = "Kích thước không được vượt quá 50 ký tự")]
        public string Size { get; set; }

        [MaxLength(50, ErrorMessage = "Màu sắc không được vượt quá 50 ký tự")]
        public string Color { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int StockQuantity { get; set; }
    }

    public class UpdateProductDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải từ 3 đến 100 ký tự")]
        public string Name { get; set; }

        [StringLength(2000, ErrorMessage = "Mô tả không được vượt quá 2000 ký tự")]
        public string Description { get; set; }

        [Range(0.01, 1000000000, ErrorMessage = "Giá phải lớn hơn 0 và nhỏ hơn 1 tỷ")]
        public decimal? Price { get; set; }

        public int? CategoryId { get; set; }

        [Url(ErrorMessage = "URL hình ảnh không hợp lệ")]
        public string ImageUrl { get; set; }
        // Biến thể không cần ProductId vì nó sẽ được gán tự động
        public List<CreateProductVariantInlineDto> Variants { get; set; } = new List<CreateProductVariantInlineDto>();
    }

    public class UpdateProductVariantDto
    {
        public int VariantId { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        [Range(0, 1000000, ErrorMessage = "Giá bổ sung phải từ 0 đến 1 triệu")]
        public decimal? AdditionalPrice { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")]
        public int StockQuantity { get; set; }

        [Url(ErrorMessage = "URL hình ảnh biến thể không hợp lệ")]
        public string VariantImageUrl { get; set; }
    }

    public class ProductFilterOptions
    {
        public int? CategoryId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá tối thiểu không được âm")]
        public decimal? MinPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá tối đa không được âm")]
        public decimal? MaxPrice { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string SortBy { get; set; } = "newest"; // newest, price-asc, price-desc, name-asc, name-desc

        public string Keyword { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số trang phải lớn hơn 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Kích thước trang phải từ 1 đến 100")]
        public int PageSize { get; set; } = 10;

        public bool IncludeDeleted { get; set; } = false;
    }

    

    // Generic Paging DTO
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    }
    public class ProductWithDiscountDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountedPrice { get; set; }
        public DateTime DiscountStartDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public bool IsActive => DateTime.UtcNow >= DiscountStartDate && DateTime.UtcNow <= DiscountEndDate;
    }
}