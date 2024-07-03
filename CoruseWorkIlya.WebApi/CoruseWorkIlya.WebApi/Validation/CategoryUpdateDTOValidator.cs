using CourseWork.WebApi.Models.DTOs;
using FluentValidation;

namespace CourseWork.WebApi.Validation
{
    public class CategoryUpdateDTOValidator : AbstractValidator<CategoryUpdateDTO>
    {
        public CategoryUpdateDTOValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(30);
        }
    }
}
