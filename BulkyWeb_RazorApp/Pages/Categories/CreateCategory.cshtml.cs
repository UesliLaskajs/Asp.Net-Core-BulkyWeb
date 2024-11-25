using BulkyWeb_RazorApp.Data;
using BulkyWeb_RazorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_RazorApp.Pages.Categories
{
    public class CreateCategoryModel : PageModel
    {
        
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public Category Category { get; set; }
        
        public CreateCategoryModel(ApplicationDbContext db)
        {
            _db= db;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.categories.Add(Category);
                _db.SaveChanges();
                return RedirectToPage("Index"); // Correctly redirect to the Index Razor Page
            }
            return Page(); // Return the same page if validation fails
        }

    }
}
