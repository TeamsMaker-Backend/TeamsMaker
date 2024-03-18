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
            .OwnsOne(x => x.CircleInfoPermissions, x =>
            {
                x.Property(x => x.UpdateFiles).HasColumnName("UpdateFiles");
                x.Property(x => x.UpdateInfo).HasColumnName("UpdateInfo");
            });

        builder
            .HasOne(x => x.CircleMember)
            .WithOne(y => y.Permission)
            .HasForeignKey<Permission>(y => y.CircleMemberId);
    }
}
