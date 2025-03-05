using Microsoft.EntityFrameworkCore;
using Web.Models.Domain;

namespace Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
    }

}
