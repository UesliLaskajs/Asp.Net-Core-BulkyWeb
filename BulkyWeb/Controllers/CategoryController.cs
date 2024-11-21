using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;  // Make sure to include this

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller  // Inherited from Controller
    {
        private readonly ApplicationDbContext _db;  // Access field for storing the Database Context

        // Constructor to inject the DbContext into the controller
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;  // Store the DbContext in a private field
        }

        // Action method to handle the Index view and return a list of categories
        public IActionResult Index()//IActionResult is the base type for all the result types in ASP.NET Core MVC. 
        {
            // Fetch the categories from the database (using the correct 'Categories' property)
            List<Category> categoryList = _db.categories.ToList();

            // Pass the categories to the view
            return View(categoryList);
        }

        public IActionResult Create()//Action Controller To Create And Direct to the Create Page
        {
            return View();
        }
        [HttpPost] //The Same Controller Is Making A Post Request Where in The Ui is submiting a form
        public IActionResult Create(Category modelTypeCat)//Type of Category Model createing an Object from Form
        {
            if (ModelState.IsValid)//Catching Erros
            {
                _db.Add(modelTypeCat);//Adding the form Inputs to Db
                _db.SaveChanges();//Saving 
                return RedirectToAction("Index");//Redirects to Dashboard where we are displaying data from database
            }
           
            
            return View();//If Error it Redirects to Same Page
        }
    }
}
