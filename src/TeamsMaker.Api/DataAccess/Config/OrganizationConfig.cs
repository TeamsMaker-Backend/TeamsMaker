using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class OrganizationConfig : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable(nameof(Organization), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder
            .OwnsOne(x => x.Name)
            .HasData(LoadOwned());

        builder
            .OwnsOne(x => x.Logo);

        builder
            .HasMany(x => x.Users)
            .WithOne(y => y.Organization)
            .HasForeignKey(y => y.OrganizationId);

        builder
            .HasMany(x => x.Roles)
            .WithOne(y => y.Organization)
            .HasForeignKey(y => y.OrganizationId);

        builder
            .HasMany(x => x.Circles)
            .WithOne(y => y.Organization)
            .HasForeignKey(y => y.OrganizationId);

        builder
            .HasMany(x => x.Departments)
            .WithOne(y => y.Organization)
            .HasForeignKey(y => y.OrganizationId);

        builder.HasData(LoadData());
    }

    static object[] LoadOwned()
    {
        return [
            new
            {
                OrganizationId = 1,
                Eng = "Computers and Information Systems Kafr-Elsheikh University",
                Loc = "الحاسبات ونظم المعلومات جامعة كفر الشيخ",
            }
        ];
    }

    static object[] LoadData()
    {
        return [
            new
            {
                Id = 1,
                Address = "Kafr-Elsheikh City",
                IsActive = true
            }
        ];
    }
}
