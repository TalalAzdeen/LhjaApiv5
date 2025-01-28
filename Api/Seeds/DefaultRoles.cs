using Entities;
using Microsoft.AspNetCore.Identity;
using Utilities;

namespace Api.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            if (await roleManager.RoleExistsAsync(UserRole.Admin))
            {
                return;
            }
            await roleManager.CreateAsync(new ApplicationRole { Name = UserRole.Admin });
            await roleManager.CreateAsync(new ApplicationRole { Name = UserRole.SuperVisor });
            await roleManager.CreateAsync(new ApplicationRole { Name = UserRole.User });
        }



    }
}