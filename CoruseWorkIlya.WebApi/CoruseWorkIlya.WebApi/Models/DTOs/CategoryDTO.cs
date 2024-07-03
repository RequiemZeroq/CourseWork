using System.ComponentModel.DataAnnotations;

namespace CourseWork.WebApi.Models.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
            = default!;
    }
}
