using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface IProduct : IRepository<Product> //Class That Implements IRepo And Product Class 
    {
        void Update(Product product);//Only Method that cannot be inserted in interface 
    }
}
