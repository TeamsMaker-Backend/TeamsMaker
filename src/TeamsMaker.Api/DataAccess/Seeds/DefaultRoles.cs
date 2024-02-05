using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Api.Core.Guards;
using TeamsMaker.Api.DataAccess.Context;

namespace TeamsMaker.Api.DataAccess.Seeds;

public static class DefaultRoles
{
    public static async Task SeedRoles(AppDBContext db, RoleManager<Role> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            var organization = await db.Organizations.FirstOrDefaultAsync();

            Guard.Against.Null(organization, nameof(organization));

            await roleManager.CreateAsync(new Role()
            {
                Name = AppRoles.Admin,
                NormalizedName = AppRoles.Admin.ToUpper(),
                IsOrganizationAdmin = true,
                OrganizationId = organization!.Id
            });

            foreach (var newRole in AppRoles.OrdinaryRoles)
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = newRole,
                    OrganizationId = organization!.Id,
                    Organization = organization
                });
            }
        }
    }
}
