namespace CourseWork.WebApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
            = default!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

}
