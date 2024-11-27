using Bulky.Models;
using Microsoft.EntityFrameworkCore;


namespace Bulky.DataAccess.Data
{
    public class ApplicationDbContext:DbContext// Created a class Wich Inherists from DatabaseContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base (options) 
        {
            // The base constructor call here passes the options to the DbContext base class
            // This allows you to configure your database context based on the settings in the options
        }


        public DbSet<Category> categories { get; set; }//Created a Set Type Wich Creates a Categories Table


        protected override void OnModelCreating(ModelBuilder modelBuilder) //Override an Implemented Method for Modifiying The Model Entity
        {

            modelBuilder.Entity<Category>().HasData(//Modifing The Table by the Category model
                new Category { Id = 1, Name = "Action", CategoryOrder = 3 },//Inserting Category Class 
                new Category { Id = 2, Name = "Drama", CategoryOrder = 4 },
                new Category { Id = 3, Name = "Sci-Fi", CategoryOrder = 1 }

                );
        }
    }
}
//Add-migrations
//Update-Database