using AutoMapper;
using CourseWork.WebApi.Models;
using CourseWork.WebApi.Models.DTOs;

namespace CourseWork.WebApi.Mapping
{
    public class ProductMappingConfig : Profile
    {
        public ProductMappingConfig()
        {
            CreateMap<Product, ProductDTO>()
                .ReverseMap();
            CreateMap<Product, ProductCreateDTO>()
                .ReverseMap();
            CreateMap<Product, ProductUpdateDTO>()
                .ReverseMap();
        }
    }
}
