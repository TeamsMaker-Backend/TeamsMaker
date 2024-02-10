using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role), DatabaseSchemas.Lookups);

        builder
            .HasOne(x => x.Organization)
            .WithMany(y => y.Roles)
            .HasForeignKey(x => x.OrganizationId);
    }
}
