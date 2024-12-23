﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
  

        public ICategory CategoryRepo { get; set; }

        public UnitOfWork(ApplicationDbContext db)
        {
         
            _db = db;
            
        }

       public void Save()
        {
            _db.SaveChanges();
        }
    }
}
