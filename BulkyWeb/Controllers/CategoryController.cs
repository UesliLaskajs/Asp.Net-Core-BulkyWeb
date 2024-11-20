using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;
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
    }
}
