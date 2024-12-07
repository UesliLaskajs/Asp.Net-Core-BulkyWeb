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
        }
        public void Add(T entity) //Adding The (T) to db
        {
           dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter) //Gets Item in Db
        {
            IQueryable<T> query = dbSet;//Creates A query 
            query=query.Where(filter);//Selects The Filtering Part
            return query.FirstOrDefault();//Returns the founded Filter 
        }

        public IEnumerable<T> GetAll()//Gets All From IQueryable
        {
            IQueryable<T> query = dbSet;//IQueryable is used for building a query to the database that can be executed later.
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
