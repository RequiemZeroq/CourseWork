using System.Linq.Expressions;

namespace CourseWork.WebApi.Data.Repository.IRepository
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            string? includeProperties = null);
        Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            bool tracked = true,
            string? includeProperties = null);
        Task CreateAsync(TEntity entity);
        Task RemoveAsync(TEntity entity);
    }
}
