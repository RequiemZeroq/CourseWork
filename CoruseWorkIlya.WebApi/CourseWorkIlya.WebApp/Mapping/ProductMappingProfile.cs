using AutoMapper;
using CourseWork.WebApp.Models.DTOs;

namespace CourseWork.WebApp.Mapping
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<ProductUpdateDTO, ProductDTO>()
                .ReverseMap();
        }
    }
}
