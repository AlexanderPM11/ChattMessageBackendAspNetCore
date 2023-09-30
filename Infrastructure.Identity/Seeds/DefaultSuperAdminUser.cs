using crudSegnalR.Infrastructure.Identity.Entities;
using crudSignalR.Core.Application.Enums.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSegnalR.Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdminUser
    {
        public static async Task SeedAsync(UserManager<ApplicantionUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicantionUser defaultUser = new();

            defaultUser.UserName = "SuperAdminUser";
            defaultUser.Email = "SuperAdminUser@gmail.com";
            defaultUser.FirstName = "Johg";
            defaultUser.LastName = "Super Admin 02";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(user => user.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pass$$Word!");
                    await userManager.AddToRoleAsync(defaultUser, UserRoles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, UserRoles.SuperAdmin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, UserRoles.Admin.ToString());
                }
            }

        }
    }
}
