using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategory
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var objFromDb = _db.categories.FirstOrDefault(c => c.Id == category.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
                objFromDb.CategoryOrder = category.CategoryOrder;
                // Update other fields as needed
            }
        }
    }
}
