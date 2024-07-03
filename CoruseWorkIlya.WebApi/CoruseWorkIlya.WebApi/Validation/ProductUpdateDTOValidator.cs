using CourseWork.WebApi.Models.DTOs;
using FluentValidation;

namespace CourseWork.WebApi.Validation
{
    public class ProductUpdateDTOValidator : AbstractValidator<ProductUpdateDTO>
    {
        public ProductUpdateDTOValidator() 
        {
            RuleFor(p => p.Id).NotEmpty();
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Price).NotEmpty();
            RuleFor(p => p.Description).NotEmpty(); 
            RuleFor(p => p.CategoryId).NotEmpty();
        }
    }
}
