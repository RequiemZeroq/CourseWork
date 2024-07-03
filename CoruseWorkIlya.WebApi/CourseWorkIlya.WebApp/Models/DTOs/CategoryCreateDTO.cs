using System.ComponentModel.DataAnnotations;

namespace CourseWork.WebApp.Models.DTOs
{
    public class CategoryCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
            = default!;
    }
}
