using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace Beer_CodeFirst.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Thêm các trường mới
        public bool IsEmailConfirmed { get; set; } = false;
        public string EmailConfirmationToken { get; set; }
        public DateTime? TokenExpiryDate { get; set; }

        // Bạn có thể thêm các trường khác nếu cần
        public string FullName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    // Cập nhật DbContext
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("BeerStringEntities") // Tên connection string của bạn
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Thêm các DbSet khác của bạn ở đây
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        // ... các DbSet khác
    }
}