using CourseWork.WebApi.Models;
using CourseWork.WebApi.Models.DTOs;
using FluentValidation;

namespace CourseWork.WebApi.Validation
{
    public class CategoryCreateDTOValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CategoryCreateDTOValidator() 
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        }
    }
}
