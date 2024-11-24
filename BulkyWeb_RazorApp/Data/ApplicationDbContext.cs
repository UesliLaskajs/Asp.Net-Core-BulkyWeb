using BulkyWeb_RazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb_RazorApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

       public DbSet<Category> categories { get; set; }

        protected  override void  OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                 new Category { Id = 1, Name = "Action", CategoryOrder = 3 },//Inserting Category Class 
                new Category { Id = 2, Name = "Drama", CategoryOrder = 4 },
                new Category { Id = 3, Name = "Sci-Fi", CategoryOrder = 1 }
                );

        }

    }
}
