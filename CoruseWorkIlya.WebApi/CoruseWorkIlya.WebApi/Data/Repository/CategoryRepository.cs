using CourseWork.WebApi.Data.Repository.IRepository;
using CourseWork.WebApi.Models;

namespace CourseWork.WebApi.Data.Repository
{
    public class CategoryRepository :
        Repository<Category>,
        ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) 
            : base(db)
        {
            _db = db;
        }

        public Task<Category> UpdateAsync(Category entity)
        {
            entity.UpdatedDate = DateTime.Now;  
            _db.Categories.Update(entity);
            return Task.FromResult(entity);
        }
    }
}
