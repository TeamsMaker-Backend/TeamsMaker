using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;


namespace TeamsMaker.Api.DataAccess.Config;

public class CircleMemberConfig : IEntityTypeConfiguration<CircleMember>
{
    public void Configure(EntityTypeBuilder<CircleMember> builder)
    {
        builder.ToTable(nameof(CircleMember), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .HasMaxLength(200);

        builder
            .HasOne(x => x.Circle)
            .WithMany(y => y.CircleMembers)
            .HasForeignKey(x => x.CircleId);

        builder
            .HasOne(x => x.User)
            .WithMany(y => y.MemberOn)
            .HasForeignKey(x => x.UserId);

        builder
            .HasOne(x => x.ExceptionPermission)
            .WithOne(y => y.CircleMember)
            .HasForeignKey<Permission>(x => x.CircleMemberId);
    }
}
