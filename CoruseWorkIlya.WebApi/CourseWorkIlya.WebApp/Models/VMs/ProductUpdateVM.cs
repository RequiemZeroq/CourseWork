using CourseWork.WebApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CourseWork.WebApp.Models.VMs
{
    public class ProductUpdateVM
    {
        public ProductUpdateVM()
        {
            CategoryListItems = new List<SelectListItem>();
        }
        public ProductUpdateDTO ProductUpdateModel { get; set; }
        public IEnumerable<SelectListItem> CategoryListItems { get; set; }
    }
}
