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

        builder
            .OwnsOne(x => x.Avatar);

        builder
            .OwnsOne(x => x.Header);


        builder
            .HasMany(x => x.CircleMembers)
            .WithOne(y => y.Circle)
            .HasForeignKey(y => y.CircleId);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.Status)
            .HasDefaultValue(CircleStatusEnum.Active);
    }
}
