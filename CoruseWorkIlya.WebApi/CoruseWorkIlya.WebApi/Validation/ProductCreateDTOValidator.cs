using CourseWork.WebApi.Models.DTOs;
using FluentValidation;

namespace CourseWork.WebApi.Validation
{
    public class ProductCreateDTOValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateDTOValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Description).NotEmpty(); 
            RuleFor(p => p.Price).NotEmpty();
            RuleFor(p => p.CategoryId).NotEmpty();
        }
    }
}
