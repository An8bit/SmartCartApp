using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.DTO.CategoryDTOs
{
    // DTO chính để hiển thị thông tin category đầy đủ
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public string ParentCategoryName { get; set; }

        public int ProductCount { get; set; }

        public List<CategoryBasicDto> SubCategories { get; set; } = new List<CategoryBasicDto>();
    }

    // DTO cơ bản cho danh sách, dropdown hoặc các phần tử con
    public class CategoryBasicDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int? ParentCategoryId { get; set; }

        public string Description { get; set; } 

        public int ProductCount { get; set; }
    }

    // DTO để tạo mới category
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Tên danh mục là bắt buộc")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên danh mục phải từ 2 đến 100 ký tự")]
        public string CategoryName { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
    }

    // DTO để cập nhật category
    public class CategoryUpdateDto
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Tên danh mục phải từ 2 đến 100 ký tự")]
        public string CategoryName { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        public int? ParentCategoryId { get; set; }
    }

    // DTO cho việc lọc danh sách category
    public class CategoryFilterDto
    {
        public string Keyword { get; set; }

        public int? ParentCategoryId { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}