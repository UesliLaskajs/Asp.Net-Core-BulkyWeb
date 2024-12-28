using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        private ICategory _categoryRepo;
        private IProduct _productRepo;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        public ICategory Category => _categoryRepo ??= new CategoryRepository(_db);
        public IProduct Product => _productRepo ??= new ProductRepository(_db);

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
