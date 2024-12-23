using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class  //Class Repo That inherits interface and 
    {
        private readonly ApplicationDbContext _db; //Create The Dependency Injection
        internal DbSet<T> dbSet;//Create a DataTable Set
        public Repository(ApplicationDbContext db)//Constructor to set the DataBase Set
        {
            _db = db;
            this.dbSet = _db.Set<T>();// representing the table corresponding to the entity type T
            _db.products.Include(u => u.category);
        }
        public void Add(T entity) //Adding The (T) to db
        {
           dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null) //Gets Item in Db
        {
            IQueryable<T> query = dbSet;//Creates A query 
            if (string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includProp in includeProperties.Split(new char[','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }
            query =query.Where(filter);//Selects The Filtering Part
            return query.FirstOrDefault();//Returns the founded Filter 
        }

        public IEnumerable<T> GetAll(string? includeProperties=null)//Gets All From IQueryable
        {
            IQueryable<T> query = dbSet;//IQueryable is used for building a query to the database that can be executed later.
            if(string.IsNullOrEmpty(includeProperties))
            {
                foreach(var includProp in includeProperties.Split(new char[','],StringSplitOptions.RemoveEmptyEntries)) {
                    query=query.Include(includProp);
                }
            }
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity); //Removes From Db 
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
     
            dbSet.RemoveRange(entity);
        }
    }
}
