using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Core.Enums;


namespace TeamsMaker.Api.DataAccess.Config;

public class CircleConfig : IEntityTypeConfiguration<Circle>
{
    public void Configure(EntityTypeBuilder<Circle> builder)
    {
        builder.ToTable(nameof(Circle), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description).HasMaxLength(150);

        builder
            .Property(x => x.Status)
            .HasDefaultValue(CircleStatusEnum.Active)
            .HasSentinel(CircleStatusEnum.Active)
            .HasConversion<int>();

        builder
            .OwnsOne(x => x.Avatar);

        builder
            .OwnsOne(x => x.Header);

        builder
            .OwnsOne(x => x.Summary, x =>
            {
                x.Property(x => x.Summary).HasColumnName("Summary");
                x.Property(x => x.IsPublic).HasColumnName("IsSummaryPublic");
            });

        builder
            .HasOne(x => x.Organization)
            .WithMany(y => y.Circles)
            .HasForeignKey(x => x.OrganizationId);

        builder
            .HasMany(x => x.CircleMembers)
            .WithOne(y => y.Circle)
            .HasForeignKey(y => y.CircleId);

        builder
            .HasMany(x => x.Links)
            .WithOne(y => y.Circle)
            .HasForeignKey(y => y.CircleId);

        builder
            .HasMany(x => x.Skills)
            .WithOne(y => y.Circle)
            .HasForeignKey(y => y.CircleId);

        builder
            .HasMany(c => c.Sessions)
            .WithOne(s => s.Circle)
            .HasForeignKey(s => s.CircleId)
            .IsRequired();

        builder
            .HasMany(c => c.TodoTasks)
            .WithOne(s => s.Circle)
            .HasForeignKey(s => s.CircleId)
            .IsRequired();

        builder
            .HasOne(x => x.DefaultPermission)
            .WithOne(y => y.Circle)
            .HasForeignKey<Permission>(y => y.CircleId)
            .IsRequired();
    }
}
