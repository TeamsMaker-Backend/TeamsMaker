using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class ProjectConfig : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable(nameof(Project), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();

        builder.Property(x => x.Url).IsRequired();

        builder
            .HasOne(x => x.Student)
            .WithMany(y => y.Projects)
            .HasForeignKey(x => x.StudentId)
            .IsRequired();

        builder.HasMany(x => x.Skills)
            .WithOne(y => y.Project)
            .HasForeignKey(y => y.ProjectId)
            .IsRequired();
    }
}