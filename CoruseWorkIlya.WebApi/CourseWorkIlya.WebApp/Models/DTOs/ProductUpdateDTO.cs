using System.ComponentModel.DataAnnotations;

namespace CourseWork.WebApp.Models.DTOs
{
    public class ProductUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
            = default!;
        [Required]
        public string Description { get; set; }
            = default!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Range(1, 5)]
        public double? Rate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageLocalPath { get; set; }
        //public IFormFile? Image { get; set;}
    }
}
