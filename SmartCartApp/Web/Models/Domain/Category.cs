using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Web.Models.Domain
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; } 
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }  // Danh mục cha (đệ quy)

        [JsonIgnore]  
        public  Category? ParentCategory { get; set; } 

        [JsonIgnore] 
        public  ICollection<Category>? SubCategories { get; set; }

        [JsonIgnore] 
        public  ICollection<Product>? Products { get; set; }
    }
}
