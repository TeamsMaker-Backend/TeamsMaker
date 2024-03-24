using Microsoft.Extensions.Configuration;

using TeamsMaker.Api.DataAccess.Context;

namespace TeamsMaker.Api.DataAccess.Seeds;

public static class SeedDB
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<AppDBContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var x = serviceProvider.GetRequiredService<IConfiguration>();
        System.Console.WriteLine(x.GetConnectionString("DefaultConnection"));

        db.Database.EnsureCreated();


        await DefaultRoles.SeedRoles(db, roleManager);
        await DefaultUsers.SeedAdminUser(db, userManager);
    }
}
