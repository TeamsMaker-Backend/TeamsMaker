using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamsMaker.Api.DataAccess.Config;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.Property(x => x.CollegeId)
            .IsRequired()
            .HasMaxLength(50);


    }
}
