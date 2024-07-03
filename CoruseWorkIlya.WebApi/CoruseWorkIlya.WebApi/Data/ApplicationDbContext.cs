using CourseWork.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Electronics",
                    CreatedDate = DateTime.UtcNow
                },
                new Category
                {
                    Id = 2,
                    Name = "Clothing",
                    CreatedDate = DateTime.UtcNow
                }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Smartphone",
                    Description = "High-performance smartphone with advanced features",
                    Price = 599.99m,
                    CategoryId = 1,
                    Rate = 4.5,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 2,
                    Name = "Laptop",
                    Description = "Lightweight laptop for productivity on the go",
                    Price = 1099.99m,
                    CategoryId = 1,
                    Rate = 4.2,
                    CreatedDate = DateTime.Now
                },
                new Product
                {
                    Id = 3,
                    Name = "T-shirt",
                    Description = "Casual cotton T-shirt for everyday wear",
                    Price = 19.99m,
                    CategoryId = 2,
                    Rate = 4.0,
                    CreatedDate = DateTime.Now
                }
            );

        }
    }
}
