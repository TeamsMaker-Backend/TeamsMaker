using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;

namespace TeamsMaker.Api.DataAccess.Config;

public class StaffConfig : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable(nameof(Staff), DatabaseSchemas.Dbo);

        builder
            .HasOne(x => x.User)
            .WithMany(y => y.Staff)
            .HasForeignKey(x => x.Id)
            .IsRequired();

        builder
            .HasMany(x => x.DepartmentStaff)
            .WithOne(y => y.Staff)
            .HasForeignKey(y => y.StaffId);
    }
}
