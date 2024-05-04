using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class StaffConfig : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable(nameof(Staff), DatabaseSchemas.Dbo);

        // Enums
        builder.Property(x => x.Classification)
            .HasConversion<int>();
        
        builder
            .HasMany(x => x.DepartmentStaff)
            .WithOne(y => y.Staff)
            .HasForeignKey(y => y.StaffId);

        builder
            .HasMany(x => x.ApprovalRequests)
            .WithOne(y => y.Staff)
            .HasForeignKey(y => y.StaffId)
            .IsRequired(true);
    }
}
