using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Seeds;

public static class DefaultUsers
{
    public static async Task SeedAdminUser(UserManager<User> userManger)
    {
        Staff admin = new()
        {
            FirstName = "Default",
            LastName = "User",
            UserName = "default_user",
            Email = "default_user123@gmail.com",
            EmailConfirmed = true,
            Classification = StaffClassificationsEnum.Professor
        };

        var user = await userManger.FindByEmailAsync(admin.Email);

        if (user is null)
        {
            await userManger.CreateAsync(admin, "P@ssword123");
            await userManger.AddToRoleAsync(admin, AppRoles.Admin);
        }
    }
}
