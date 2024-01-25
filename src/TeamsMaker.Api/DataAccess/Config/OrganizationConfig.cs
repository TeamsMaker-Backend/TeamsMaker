using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.DataAccess.Models;

namespace TeamsMaker.Api.DataAccess.Config;

public class OrganizationConfig : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(x => x.Id);

        builder.OwnsOne(x => x.Name, x =>
        {
            x.Property(t => t.Eng).HasColumnName("EngName");
            x.Property(t => t.Loc).HasColumnName("LocName");
        });

        builder
            .HasMany(x => x.Users)
            .WithOne(y => y.Organization)
            .HasForeignKey(x => x.OrganizationId);

        builder
            .HasMany(x => x.Roles)
            .WithOne(y => y.Organization)
            .HasForeignKey(x => x.OrganizationId);
    }
}
