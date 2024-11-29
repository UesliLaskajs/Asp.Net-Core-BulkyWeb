using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;  // Make sure to include this

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            if (modelTypeCat.Name == modelTypeCat.CategoryOrder.ToString())
            {
                ModelState.AddModelError("Name", "The DisplayOrder Cannot be the same as Name");
            }

            if (ModelState.IsValid)//Catching Erros
            {
                _db.Add(modelTypeCat);//Adding the form Inputs to Db
                _db.SaveChanges();//Saving 
                TempData["Success"] = "Created Succesful Data";
                return RedirectToAction("Index");//Redirects to Dashboard where we are displaying data from database
            }


            return View();//If Error it Redirects to Same Page
        }

        public IActionResult Edit(int? id) //Initialize an Integer to match id of View
        {
            if (id == null || id == 0)//Catch Errors
            {
                return NotFound();
            }

            Category editedItem = _db.categories.Find(id); //Find Id In Database
            //Category editedItem2 = _db.categories.FirstOrDefault(u => u.Id == id);
            //Category editedItem1 = _db.categories.Where(n => n.Id == id).FirstOrDefault();
            if (editedItem.Id == null || editedItem.Id == 0)
            {
                return NotFound();
            }
            return View(editedItem); //Return The Found Column Category
        }

        [HttpPost]
        public IActionResult Edit(Category obj)//Post For the Same Acition Edit To Update
        {
            if (ModelState.IsValid)//If Model Is Correct
            {
                _db.categories.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category findDeletion = _db.categories.Find(id);

            if (findDeletion.Id == null || findDeletion.Id == 0)
            {
                return NotFound();
            }

            return View(findDeletion);
        }

        [HttpPost, ActionName("Delete")] //The Delete Post is Called with Delete Action naming
        public IActionResult DeletePost(Category obj)
        {

            _db.categories.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}

