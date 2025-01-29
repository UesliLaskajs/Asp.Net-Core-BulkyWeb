
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CompanyRepository:Repository<Company>,ICompany
    {
        private readonly ApplicationDbContext _db; 


        public CompanyRepository( ApplicationDbContext db ):base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var objFromDb_Company=_db.Company.FirstOrDefault(item=>item.Id==company.Id);

            if(objFromDb_Company != null)
            {
                objFromDb_Company.Id = company.Id;
                objFromDb_Company.Name = company.Name;
                objFromDb_Company.StreetAdress = company.StreetAdress;
                objFromDb_Company.PhoneNumber=company.PhoneNumber;
                objFromDb_Company.PostalCode=company.PostalCode;
                objFromDb_Company.City = company.City;
                objFromDb_Company.State = company.State;
            }
        }
    }
}
