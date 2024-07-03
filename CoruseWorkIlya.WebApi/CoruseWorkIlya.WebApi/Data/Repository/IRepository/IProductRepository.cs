using CourseWork.WebApi.Models;

namespace CourseWork.WebApi.Data.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> UpdateAsync(Product entity);
    }
}
