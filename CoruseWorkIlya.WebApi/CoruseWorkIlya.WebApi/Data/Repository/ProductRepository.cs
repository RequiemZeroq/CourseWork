using CourseWork.WebApi.Data.Repository.IRepository;
using CourseWork.WebApi.Models;

namespace CourseWork.WebApi.Data.Repository
{
    public class ProductRepository :
        Repository<Product>,
        IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db)
            :base(db)
        {
            _db = db;
        }

        public Task<Product> UpdateAsync(Product entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.Products.Update(entity);
            return Task.FromResult(entity);
        }
    }
}
