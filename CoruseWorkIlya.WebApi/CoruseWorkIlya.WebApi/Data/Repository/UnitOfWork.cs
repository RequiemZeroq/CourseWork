using CourseWork.WebApi.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.WebApi.Data.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _db;
        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Products = new ProductRepository(db);
            Categories = new CategoryRepository(db);    
        }

        public async Task Commit()
        {
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();  
        }
    }
}
