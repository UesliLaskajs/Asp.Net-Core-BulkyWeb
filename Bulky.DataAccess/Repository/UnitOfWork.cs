using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        private ICategory _categoryRepo;
        private IProduct _productRepo;
        private ICompany _companyRepo;
        private IShoppingCart _shoppingCartRepo;
        private IApplicationUser _userRepo;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
        }

        

        public ICategory Category => _categoryRepo ??= new CategoryRepository(_db);
        public IProduct Product => _productRepo ??= new ProductRepository(_db);

        public ICompany Company => _companyRepo ??= new CompanyRepository(_db);

        public IShoppingCart ShoppingCart => _shoppingCartRepo ??= new ShoppingCartRepository(_db);

        public IApplicationUser ApplicationUser => _userRepo ??= new ApplicationUserRepository(_db);
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
