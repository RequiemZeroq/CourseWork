using CourseWork.WebApi.Models;

namespace CourseWork.WebApi.Data.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> UpdateAsync(Category entity);
    }
}
