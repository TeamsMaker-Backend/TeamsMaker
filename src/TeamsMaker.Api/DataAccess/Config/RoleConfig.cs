using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.DataAccess.Models;

namespace TeamsMaker.Api.DataAccess.Config;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Primary Key
        builder.Property(x => x.Id)
            .HasConversion(
                ulid => ulid.ToString(),  // Convert ULID to string for the database
                str => Ulid.Parse(str)    // Convert string from the database to ULID
            )
            .IsRequired();

        builder
            .HasOne(x => x.Organization)
            .WithMany(y => y.Roles)
            .HasForeignKey(x => x.OrganizationId);
    }
}
