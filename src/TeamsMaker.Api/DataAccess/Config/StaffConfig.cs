using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TeamsMaker.Api.DataAccess.Config;

public class StaffConfig : IEntityTypeConfiguration<Staff>
{
    public void Configure(EntityTypeBuilder<Staff> builder)
    {
        builder.ToTable("Staff");
    }
}
