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


            Role admin = new()
            {
                Name = AppRoles.Admin,
                NormalizedName = AppRoles.Admin.ToUpper(),
                IsOrganizationAdmin = true,
                OrganizationId = organization!.Id,
                Organization = organization
            };

            await db.Roles.AddAsync(admin);
            await db.SaveChangesAsync();

            foreach (var role in AppRoles.OrdinaryRoles)
            {
                // add & save
                await roleManager.CreateAsync(new Role
                {
                    Name = role,
                    OrganizationId = organization!.Id,
                    Organization = organization
                });
            }
        }
    }
}
