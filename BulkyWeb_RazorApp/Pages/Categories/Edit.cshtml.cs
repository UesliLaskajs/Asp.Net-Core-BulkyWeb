using BulkyWeb_RazorApp.Data;
using BulkyWeb_RazorApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWeb_RazorApp.Pages.Categories
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Category Category { get; set; }

        public EditModel(ApplicationDbContext db)
        {
            _db = db;
        }

        // OnGet method to fetch the category to be edited
        public IActionResult OnGet(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category = _db.categories.Find(id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        // OnPost method to handle form submission and update the category
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _db.categories.Update(Category);
                _db.SaveChanges();
                return RedirectToPage("Index");
            }

            return Page();
        }
    }
}
