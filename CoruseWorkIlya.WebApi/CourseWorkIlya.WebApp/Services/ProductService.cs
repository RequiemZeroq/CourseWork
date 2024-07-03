using CourseWork.WebApp.Models;
using CourseWork.WebApp.Models.DTOs;
using CourseWork.WebApp.Services.IServices;
using CourseWork.Utility;
using Humanizer;

namespace CourseWork.WebApp.Services
{
    public class ProductService : 
        BaseService,
        IProductService
    {
        public ProductService(IHttpClientFactory httpClient,
               IHttpContextAccessor httpContext,
               IConfiguration configuration) 
            : base(httpClient, httpContext)
        {
            ServiceUrl = configuration["ServicesUrls:MyApiUrl"]!;
        }

        protected override string ServiceUrl { get; set; }

        public Task<T> CreateAsync<T>(ProductCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiMethod = ApiMethod.POST,
                Data = dto,
                Url = ServiceUrl + "api/Product"
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiMethod = ApiMethod.DELETE,
                Url = ServiceUrl + $"api/Product/{id}"
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiMethod = ApiMethod.GET,
                Url = ServiceUrl + "api/Product"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiMethod = ApiMethod.GET,
                Url = ServiceUrl + $"api/Product/{id}"
            });
        }

        public Task<T> UpdateAsync<T>(ProductUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest
            {
                ApiMethod = ApiMethod.PUT,
                Url = ServiceUrl + "api/Product",
                Data = dto
            });
        }
    }
}
