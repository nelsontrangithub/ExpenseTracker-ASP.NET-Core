using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public static class Seed
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider,
            IConfiguration Configuration)
        {
            // adding customs roles
            var RoleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager =
                serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] roleNames = { "Admin", "Member" };

            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Get admin secrets from appsettings.json
            string adminUser = Configuration.GetSection("AdminUserSettings")["UserEmail"];
            string adminPwd =
                   Configuration.GetSection("AdminUserSettings")["UserPassword"];

            // creating a super user who could maintain the web app
            var poweruser = new ApplicationUser
            {
                UserName = adminUser,
                Email = adminUser
            };

            string userPassword = adminPwd;

            // See if admin user is already in the database
            var user = await UserManager.FindByEmailAsync(adminUser);

            if (user == null)
            {
                var createPowerUser = await UserManager.CreateAsync(poweruser,
                                                                    userPassword);
                if (createPowerUser.Succeeded)
                {
                    // here we assign the new user the "Admin" role 
                    await UserManager.AddToRoleAsync(poweruser, "Admin");
                }
            }
        }
    }
}
