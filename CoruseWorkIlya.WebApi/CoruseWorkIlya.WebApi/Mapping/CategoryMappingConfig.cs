using AutoMapper;
using CourseWork.WebApi.Models;
using CourseWork.WebApi.Models.DTOs;

namespace CourseWork.WebApi.Mapping
{
    public class CategoryMappingConfig : Profile
    {
        public CategoryMappingConfig()
        {
            CreateMap<Category, CategoryDTO>()
                .ReverseMap();
            CreateMap<Category, CategoryCreateDTO>()
                .ReverseMap();
            CreateMap<Category, CategoryUpdateDTO>()
                .ReverseMap();
        }
    }
}
