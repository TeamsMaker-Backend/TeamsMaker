using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;


namespace TeamsMaker.Api.DataAccess.Config;

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission), DatabaseSchemas.Lookups);

        builder.HasKey(x => x.Id);

        builder
            .HasMany(x => x.MemberPermissions)
            .WithOne(y => y.Permission)
            .HasForeignKey(y => y.PermissionId);
    }
}
