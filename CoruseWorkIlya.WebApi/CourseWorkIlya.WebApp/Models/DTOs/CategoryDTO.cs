using System.ComponentModel.DataAnnotations;

namespace CourseWork.WebApp.Models.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
            = default!;
    }
}
