using TeamsMaker.Api.DataAccess.Context;

namespace TeamsMaker.Api.DataAccess.Seeds;

public class DefaultOrganization
{
    public static async Task SeedOrganization(AppDBContext db)
    {
        if(!db.Organizations.Any())
        {
            Organization organization = new()
            {
                Name = new("Computers and Information Systems Kafr-Elsheikh University", "الحاسبات ونظم المعلومات جامعة كفر الشيخ"),
                Address = "Kafr-Elsheikh City",
            };  

            await db.Organizations.AddAsync(organization);
            await db.SaveChangesAsync();
        }
    }
}
