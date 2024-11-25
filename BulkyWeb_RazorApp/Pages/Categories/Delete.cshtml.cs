using BulkyWeb_RazorApp.Data;
using BulkyWeb_RazorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_RazorApp.Pages.Categories
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // Fetch the category to confirm deletion
        public void OnGet(int? id)
        {
            if (id != null || id != 0)
            {
                Category = _db.categories.Find(id);
            }

           

        }

        // Handle the delete operation
        public IActionResult OnPost()
        {
            Category? obj = _db.categories.Find(Category.Id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.categories.Remove(obj);
            _db.SaveChanges();
            return RedirectToPage("Index");
        }
    }
}
