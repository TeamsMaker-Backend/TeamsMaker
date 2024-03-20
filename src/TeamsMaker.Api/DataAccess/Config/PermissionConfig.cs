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
            .HasOne(x => x.Circle)
            .WithOne(y => y.DefaultPermission)
            .HasForeignKey<Permission>(x => x.CircleId)
            .IsRequired();

        builder
            .HasOne(x => x.CircleMember)
            .WithOne(y => y.ExceptionPermission)
            .HasForeignKey<Permission>(y => y.CircleMemberId);
    }
}
