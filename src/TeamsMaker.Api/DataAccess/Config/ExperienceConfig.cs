using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class ExperienceConfig : IEntityTypeConfiguration<Experience>
{
    public void Configure(EntityTypeBuilder<Experience> builder)
    {
        builder.ToTable(nameof(Experience), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Organization).IsRequired();

        builder.Property(x => x.Role).IsRequired();

        builder
            .HasOne(x => x.Student)
            .WithMany(x => x.Experiences)
            .HasForeignKey(x => x.StudentId);
    }
}