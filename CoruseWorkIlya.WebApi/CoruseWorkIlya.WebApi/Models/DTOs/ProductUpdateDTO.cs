using System.ComponentModel.DataAnnotations.Schema;

namespace CourseWork.WebApi.Models.DTOs
{
    public class ProductUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
            = default!;
        public string Description { get; set; }
            = default!;
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public double? Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        //public IFormFile? Image { get; set;}
    }
}
