using AutoMapper;
using Data;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Seeds;

public static class SeedData
{
    public async static void EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var userStore = scope.ServiceProvider.GetRequiredService<IUserStore<ApplicationUser>>();

            var context = scope.ServiceProvider.GetService<DataContext>();
            var maper = scope.ServiceProvider.GetService<IMapper>();
            //await context.Database.EnsureDeletedAsync();
            await context.Database.MigrateAsync();
            //await context.Database.EnsureCreatedAsync();
            /**/
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await DefaultRoles.SeedAsync(roleManager);
            await DefaultUsers.SeedAdminAsync(userStore, userManager, roleManager);

            await DefaultPlansAndServices.SeedAsync(context, maper);

           await DefaultModals.SeedAsync(context);


        }
    }
}
