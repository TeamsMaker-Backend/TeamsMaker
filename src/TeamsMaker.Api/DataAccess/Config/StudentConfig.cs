using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class StudentConfig : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable(nameof(Student), DatabaseSchemas.Dbo);

        builder.Property(x => x.CollegeId)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .OwnsOne(x => x.CV);

        builder
            .HasOne(x => x.User)
            .WithMany(y => y.Students)
            .HasForeignKey(x => x.Id)
            .IsRequired();

        builder
            .HasOne(x => x.Department)
            .WithMany(y => y.Students)
            .HasForeignKey(x => x.DepartmentId);

        builder
           .HasMany(x => x.JoinRequests)
           .WithOne(y => y.Student)
           .HasForeignKey(y => y.StudentId)
           .IsRequired();

        builder
           .HasMany(x => x.Experiences)
           .WithOne(y => y.Student)
           .HasForeignKey(y => y.StudentId)
           .IsRequired();

        builder
            .HasMany(x => x.Projects)
            .WithOne(y => y.Student)
            .HasForeignKey(y => y.StudentId)
            .IsRequired();
    }
}
