using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dmlcafepos.Data.Concrete.EfCoreIdentity;
using Dmlcafepos.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Dmlcafepos.Models
{
    public class IdentitySeedData
    {
        private const string adminUser = "Admin_123";    
        private const string adminPassword = "Admin_123";
        private const string adminRole = "admin";
        public static async void IdentityTestUser(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<EfPosContext>();

            if(context.Database.GetAppliedMigrations().Any())
            {
                context.Database.Migrate();
            }
            var userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            // Ensure the Admin role exists
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                var role = new AppRole(adminRole);
                await roleManager.CreateAsync(role);
            }
        
            var user = await userManager.FindByNameAsync(adminUser);
            if(user == null)
            {
                user = new AppUser{
                    FullName = "Irmak Akbaba",
                    UserName = adminUser,
                    Email = "dmladmin@gmail.com",
                    PhoneNumber = "05414578948",
                    MandantID = "2b7705a7-f7e0-4f27-94cc-9f8b8d22e199"
                    
                };
                await userManager.CreateAsync(user,adminPassword);
            }
             // Ensure the user has the Admin role
            if (!await userManager.IsInRoleAsync(user, adminRole))
            {
                await userManager.AddToRoleAsync(user, adminRole);
            }

        }
    }
}