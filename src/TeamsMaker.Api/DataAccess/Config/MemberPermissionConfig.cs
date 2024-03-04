using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class MemberPermissionConfig : IEntityTypeConfiguration<MemberPermission>
{
    public void Configure(EntityTypeBuilder<MemberPermission> builder)
    {
        builder.ToTable(nameof(MemberPermission), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Permission)
            .WithMany(y => y.MemberPermissions)
            .HasForeignKey(x => x.PermissionId);

        builder
            .HasOne(x => x.CircleMember)
            .WithMany(y => y.MemberPermissions)
            .HasForeignKey(x => x.CircleMemberId);
    }
}
