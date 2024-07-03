using CourseWork.WebApp.Models;

namespace CourseWork.WebApp.Services.IServices
{
    public interface IBaseService
    {
        public Task<T> SendAsync<T>(APIRequest request);
    }
}
