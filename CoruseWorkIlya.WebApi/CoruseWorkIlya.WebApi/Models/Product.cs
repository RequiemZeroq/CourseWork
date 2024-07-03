using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.WebApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } 
            = default!;
        public string Description { get; set; } 
            = default!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } 
            = default!;
        public double? Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
