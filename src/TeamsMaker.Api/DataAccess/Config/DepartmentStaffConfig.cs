using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class DepartmentStaffConfig : IEntityTypeConfiguration<DepartmentStaff>
{
    public void Configure(EntityTypeBuilder<DepartmentStaff> builder)
    {
        builder.ToTable(nameof(DepartmentStaff), DatabaseSchemas.Dbo);



        builder
            .HasOne(x => x.Department)
            .WithMany(y => y.DepartmentStaff)
            .HasForeignKey(y => y.DepartmentId);

        builder
            .HasOne(x => x.Staff)
            .WithMany(y => y.DepartmentStaff)
            .HasForeignKey(y => y.DepartmentId);
    }
}
