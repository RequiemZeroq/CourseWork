using AutoMapper;
using CourseWork.WebApp.Models.DTOs;

namespace CourseWork.WebApp.Mapping
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryUpdateDTO, CategoryDTO>()
                .ReverseMap();
        }

    }
}
