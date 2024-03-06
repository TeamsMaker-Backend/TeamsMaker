using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class SkillConfig : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable(nameof(Skill), DatabaseSchemas.Dbo);

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Project)
            .WithMany(y => y.Skills)
            .HasForeignKey(x => x.ProjectId)
            .IsRequired();
    }
}
