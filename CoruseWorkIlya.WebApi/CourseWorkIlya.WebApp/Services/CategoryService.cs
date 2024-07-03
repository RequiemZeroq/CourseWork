using CourseWork.WebApp.Models;
using CourseWork.WebApp.Models.DTOs;
using CourseWork.WebApp.Services.IServices;
using CourseWork.Utility;
using System.Runtime.CompilerServices;

namespace CourseWork.WebApp.Services
{
    public class CategoryService :
        BaseService,
        ICategoryService

    {
        public CategoryService(
            IHttpClientFactory httpClient,
            IHttpContextAccessor httpContext,
            IConfiguration configuration) 
            : base(httpClient, httpContext)
        {
            ServiceUrl = configuration["ServicesUrls:MyApiUrl"]!;
        }

        protected override string ServiceUrl { get; set; }

        public Task<T> CreateAsync<T>(CategoryCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiMethod = ApiMethod.POST,
                Data = dto,
                Url = ServiceUrl + "api/Category",
            });
        }

        public Task<T> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest() 
            { 
                ApiMethod = ApiMethod.DELETE, 
                Url = ServiceUrl + $"api/Category/{id}"
            });
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiMethod = ApiMethod.GET,
                Url = ServiceUrl + $"api/Category"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiMethod = ApiMethod.GET,
                Url = ServiceUrl + $"api/Category/{id}"
            });
        }

        public Task<T> UpdateAsync<T>(CategoryUpdateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiMethod = ApiMethod.PUT,
                Url = ServiceUrl + $"api/Category",
                Data = dto
            });
        }
    }
}
