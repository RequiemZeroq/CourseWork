using CourseWork.WebApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseWork.WebApp.Models.VMs
{
    public class ProductCreateVM
    {
        public ProductCreateVM() 
        {
            CategoryListItems = new List<SelectListItem>();
        }
        public ProductCreateDTO ProductCreateModel { get; set; }
        public IEnumerable<SelectListItem> CategoryListItems { get; set; } 
    }
}
