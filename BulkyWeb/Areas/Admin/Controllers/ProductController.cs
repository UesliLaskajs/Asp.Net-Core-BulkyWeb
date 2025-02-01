using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModel;
using Bulky.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")] //Specify The Area Of Controller Because its neccesiary to Define The Routing As Admin for this Controller
    [Authorize(Roles=SD.Role_User_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public CompanyController(ApplicationDbContext db, IWebHostEnvironment webhostenviroment, IUnitOfWork unitOfWork)
        {
            _db = db;
            _environment = webhostenviroment;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            // Convert IEnumerable<Company> to List<Company>
            List<Company> Companys = _unitOfWork.Company.GetAll().ToList();

            return View(Companys);
        }

        public IActionResult Upsert(int? id)
        {
            // Create the CompanyVM object with an empty Company and CategoryList
            Company Company = new Company();

            // Pass the CompanyVM object to the view
            if (id == null || id == 0)
            {
                return View(Company);
            }
            else
            {
                Company = _db.Company.Find(id);
                return View(Company);
            }

        }

        [HttpPost] // Specifies that this action will handle HTTP POST requests
        public IActionResult Upsert(Company company)
        {
            // Check if the Company title is null and add a validation error if true
            if (company.Name == null)
            {
                ModelState.AddModelError("Name", "The Name Cannot Be Empty");
            }

          

                // Add the Company to the database and save changes

                if (company.Id == 0)
                {
                    _db.Company.Add(company);
                }
                else
                {
                    _db.Company.Update(company);
                }

                _db.SaveChanges();

                // Redirect to the Index page after saving the Company
                return RedirectToAction("Index");

            return View(company);
            }

            
           
        


        [HttpPost]

        public IActionResult Edit(Company Company)
        {

            if (ModelState.IsValid)
            {
                _db.Company.Update(Company);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Company);
        }

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var Companys = _unitOfWork.Company.GetAll(includeProperties: "category")
                                              .Select(p => new {
                                                  p.Id,
                                                  p.Name,
                                                  p.StreetAdress,
                                                  p.City,
                                                  p.State,
                                                  p.PostalCode,
                                                  p.PhoneNumber,
                                                  
                                              }).ToList();

            return Json(new { data = Companys });
        }

        //[HttpDelete]

        public IActionResult Delete(int? id)
        {
            // Check if the id is null
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid Company ID" });
            }

            // Fetch the Company to be deleted
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);

            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Company not found" });
            }

           

            // Remove the Company from the database
            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Company successfully deleted" });
        }



        #endregion

    }
}