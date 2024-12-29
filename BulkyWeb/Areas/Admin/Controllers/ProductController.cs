using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")] //Specify The Area Of Controller Because its neccesiary to Define The Routing As Admin for this Controller
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webhostenviroment, IUnitOfWork unitOfWork)
        {
            _db = db;
            _environment = webhostenviroment;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            // Convert IEnumerable<Product> to List<Product>
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "category").ToList();

            return View(products);
        }

        public IActionResult Upsert(int? id)
        {
            // Create the ProductVM object with an empty Product and CategoryList
            ProductVM productsVM = new ProductVM
            {
                // Populate the CategoryList from the database
                CategoryList = _db.categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }), // Convert the query result to a list
                    // Initialize the Products property with an empty Product object
                Products = new Product()
            };

            // Pass the ProductVM object to the view
            if (id == null || id == 0)
            {
                return View(productsVM);
            }
            else
            {
                productsVM.Products = _db.products.Find(id);
                return View(productsVM);
            }

        }

        [HttpPost] // Specifies that this action will handle HTTP POST requests
        public IActionResult Upsert(ProductVM producttype, IFormFile file)
        {
            // Check if the product title is null and add a validation error if true
            if (producttype.Products.Title == null)
            {
                ModelState.AddModelError("Title", "The Title Cannot Be Empty");
            }

            // Get the web root path (where the images folder is located)
            string wwwRootPath = _environment.WebRootPath;

            // Proceed if the model is valid (i.e., no validation errors)
            if (ModelState.IsValid)
            {
                // Check if a file was uploaded
                if (file != null)
                {
                    // Generate a unique file name using a GUID and retain the file extension
                    string fileName = new Guid().ToString() + Path.GetExtension(file.FileName);
                    // Define the path where the image will be saved (inside images/NewFolder2)
                    string productPath = Path.Combine(wwwRootPath, @"images\NewFolder2");

                    if (!string.IsNullOrEmpty(producttype.Products.image))
                    {
                        var oldPath = Path.Combine(wwwRootPath, producttype.Products.image.TrimStart('/'));
                        if (System.IO.File.Exists(oldPath)) {
                            System.IO.File.Delete(oldPath);
                        }
                    }



                    // Create the directory if it doesn't already exist
                    using (var filestream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        // Copy the uploaded file to the file stream (save it to the disk)
                        file.CopyTo(filestream);
                    }

                    // Set the product's image property to the relative path of the saved image
                    producttype.Products.image = @"\images\NewFolder2\" + fileName;
                }

                // Add the product to the database and save changes

                if (producttype.Products.Id == 0)
                {
                    _db.products.Add(producttype.Products);
                }
                else
                {
                    _db.products.Update(producttype.Products);
                }

                _db.SaveChanges();

                // Redirect to the Index page after saving the product
                return RedirectToAction("Index");
            }

            // If the model is not valid, return the current view with validation errors
            return View(producttype);
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

        //public IActionResult Delete(int id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }

        //    Product FindDeletion = _db.products.Find(id);

        //    return View(FindDeletion);
        //}

        //[HttpPost, ActionName("Delete")]

        //public IActionResult DeletePost(Product prod)
        //{
        //    if (prod.Id == null || prod.Title == null)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _db.products.Remove(prod);
        //        _db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(prod);
        //}

        #region Api Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "category")
                                              .Select(p => new {
                                                  p.Title,
                                                  p.ISBN,
                                                  p.Price,
                                                  p.Author,
                                                  p.CategoryId,
                                                  CategoryName = p.category.Name // Flatten the category
                                              }).ToList();

            return Json(new { data = products });
        }

        [HttpDelete]

        public IActionResult Delete(int? id)
        {
            // Check if the id is null
            if (id == null)
            {
                return Json(new { success = false, message = "Invalid product ID" });
            }

            // Fetch the product to be deleted
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);

            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Product not found" });
            }

            // Handle image deletion if exists
            if (!string.IsNullOrEmpty(productToBeDeleted.image))
            {
                var oldImg = Path.Combine(_environment.WebRootPath, productToBeDeleted.image.TrimStart('/'));

                // Check if the image file exists before deleting
                if (System.IO.File.Exists(oldImg))
                {
                    try
                    {
                        System.IO.File.Delete(oldImg);
                    }
                    catch (Exception ex)
                    {
                        // Log the error (optional) and return failure response
                        return Json(new { success = false, message = "Error deleting image: " + ex.Message });
                    }
                }
            }

            // Remove the product from the database
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Product successfully deleted" });
        }


        #endregion

    }
    }