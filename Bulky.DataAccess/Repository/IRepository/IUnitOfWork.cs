using Bulky.Models;
using Bulky.Models.Models;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategory Category { get; }
        IProduct Product { get; }

        ICompany Company { get; }

        IShoppingCart ShoppingCart { get; }

        IApplicationUser ApplicationUser { get; }

        IOrderDetails OrderDetails { get; }
        IOrderHeader OrderHeader { get; }
        void Save();
    }
}
