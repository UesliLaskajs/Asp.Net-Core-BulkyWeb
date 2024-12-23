using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork //All Repos Are Saved to UnitOfWork Wich later Can be called to any class
    {
        ICategory CategoryRepo { get; }
        void Save();
    }
}
