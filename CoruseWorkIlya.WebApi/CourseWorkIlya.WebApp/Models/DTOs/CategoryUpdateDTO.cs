using System.ComponentModel.DataAnnotations;

namespace CourseWork.WebApp.Models.DTOs
{
    public class CategoryUpdateDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
            = default!;
    }
}
