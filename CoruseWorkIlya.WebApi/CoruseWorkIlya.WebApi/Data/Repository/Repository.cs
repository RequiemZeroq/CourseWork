using CourseWork.WebApi.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourseWork.WebApi.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<TEntity> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<TEntity>();
        }

        public async Task CreateAsync(TEntity entity)
        {
            await _db.AddAsync(entity);
        }

        public async Task<List<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            string? includeProperties = null)
        {
            IQueryable<TEntity> query = dbSet; 

            if(filter is not null) 
            {
                query = query.Where(filter);
            }
            if(includeProperties is not null)
            {
                foreach(var includeProps in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProps);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            bool tracked = true,
            string? includeProperties = null)
        {
            IQueryable<TEntity?> query = dbSet;

            if(filter is not null)
            {
                query = query.Where(filter);
            }
            if(includeProperties is not null)
            {
                foreach (var includeProps in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProps);
                }
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public Task RemoveAsync(TEntity entity)
        {
            dbSet.Remove(entity);
            return Task.CompletedTask;  
        }

    }
}
