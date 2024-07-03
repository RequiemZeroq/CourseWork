namespace CourseWork.WebApp.Models.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
            = default!;
        public string Description { get; set; }
            = default!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
            = default!;
        public double? Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
    }
}
