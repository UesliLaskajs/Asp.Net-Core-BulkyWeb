using Bulky.DataAccess.Data;
using Bulky.Models.Models;
using Bulky.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.DbInitializer
{
    public class DbInitializer:IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
        {
            _userManager=userManager;
            _roleManager=roleManager;
            _db=db;

        }

        public void Initialize()
        {
            //Migrations if not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch(Exception e)
            {
                if (!_roleManager.RoleExistsAsync(SD.Role_User_Customer).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Customer)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Company)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Admin)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Employee)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "admin@gmail.com",
                        Email = "adminAdmin@gmail.com",
                        Name = "Admin",
                        PhoneNumber = "08509823754",
                        City = "Tirane",
                        State = "Albania",
                        PostalCode = "1001",
                        StreetAdress = "5 MAJI"
                    }, "Admin123*").GetAwaiter().GetResult();

                    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "adminAdmin@gmail.com");
                    _userManager.AddToRoleAsync(user, SD.Role_User_Admin).GetAwaiter().GetResult();

                }

                return;
            }



            //Create Roles if not created




            //if roles not created ,create Admin User
        }

    }
}
