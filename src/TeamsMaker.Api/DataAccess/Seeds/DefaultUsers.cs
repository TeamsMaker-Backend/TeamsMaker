using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.DataAccess.Context;

namespace TeamsMaker.Api.DataAccess.Seeds;

public static class DefaultUsers
{
    public static async Task SeedAdminUser(AppDBContext db, UserManager<User> userManger)
    {
        if (!userManger.Users.Any())
        {
            var organization = await db.Organizations.FirstOrDefaultAsync();

            if (organization is null) throw new ArgumentNullException(nameof(organization));

            Staff admin = new()
            {
                FirstName = "Default",
                LastName = "User",
                UserName = "default_user",
                Email = "default_user123@gmail.com",
                EmailConfirmed = true,
                Classification = StaffClassificationsEnum.Professor,
                OrganizationId = organization.Id
            };

            var user = await userManger.FindByEmailAsync(admin.Email);

            if (user is null)
            {
                await userManger.CreateAsync(admin, "P@ssword123");
                await userManger.AddToRoleAsync(admin, AppRoles.Admin);
            }
        }
    }
}
