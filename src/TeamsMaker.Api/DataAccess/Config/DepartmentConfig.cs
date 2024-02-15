using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class DepartmentConfig : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable(nameof(Department), DatabaseSchemas.Lookups);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Code)
            .IsRequired();

        builder
            .HasMany(x => x.Students)
            .WithOne(y => y.Department)
            .HasForeignKey(y => y.DepartmentId);

        builder
            .HasMany(x => x.DepartmentStaff)
            .WithOne(y => y.Department)
            .HasForeignKey(y => y.DepartmentId);

        builder.HasData(LoadData());
    }

    private object[] LoadData()
    {
        return [
            new
            {
                Id = 1,
                Name = "Computer Science",
                Code = "CS",
                IsActive = true
            },
            new
            {
                Id = 2,
                Name = "Information System",
                Code = "IS",
                IsActive = true
            }
        ];
    }
}
