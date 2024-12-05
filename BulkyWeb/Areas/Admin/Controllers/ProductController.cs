using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Product> products = _db.products.ToList();
            
            return View(products);
        }

        public IActionResult Create()
        {

            IEnumerable<SelectListItem> CategoryList = _db.categories.Select(c => new SelectListItem//Data That is Passed to View Temporary
            {//Temporary Data is not in the model
                Text = c.Name,
                Value = c.Id.ToString()
            });

            ViewBag.CategoryList = CategoryList;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product producttype)
        {
            if (producttype.Title == null)
            {
                ModelState.AddModelError("Title", "The Title Cannot Be Empty");
            }

            if (ModelState.IsValid)
            {
                _db.products.Add(producttype);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                ModelState.AddModelError("Id Is Null", "Id Cannot Be Null");
            }

            Product product = _db.products.Find(id);

            if (product.Id == null || product.Title == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]

        public IActionResult Edit(Product product)
        {

            if (ModelState.IsValid)
            {
                _db.products.Update(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product FindDeletion = _db.products.Find(id);

            return View(FindDeletion);
        }

        [HttpPost, ActionName("Delete")]

        public IActionResult DeletePost(Product prod)
        {
            if (prod.Id == null || prod.Title == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.products.Remove(prod);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(prod);
        }
    }
}