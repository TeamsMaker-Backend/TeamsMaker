using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using TeamsMaker.Api.DataAccess.Models;
using TeamsMaker.Core.Enums; // Add this using directive

namespace TeamsMaker.Api.DataAccess.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Primary Key
        builder.Property(x => x.Id)
            .HasConversion(
                ulid => ulid.ToString(),  // Convert ULID to string for the database
                str => Ulid.Parse(str)    // Convert string from the database to ULID
            )
            .IsRequired();

        // Properties
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(255);
        builder.Property(x => x.SSN).IsRequired().HasMaxLength(20);
        builder.Property(x => x.CollegeId).IsRequired().HasMaxLength(50);
        builder.Property(x => x.GPA).HasColumnType("float");
        builder.Property(x => x.Avatar).HasColumnType("varbinary(max)");
        builder.Property(x => x.Header).HasColumnType("varbinary(max)");
        builder.Property(x => x.Bio).HasMaxLength(500);
        builder.Property(x => x.About).HasMaxLength(2000);
        builder.Property(x => x.GraduationYear).HasColumnType("date");
        builder.Property(x => x.CV).HasColumnType("varbinary(max)");
        builder.Property(x => x.City).HasMaxLength(255);

        // Relationships
        builder
            .HasOne(x => x.Organization)
            .WithMany(y => y.Users)
            .HasForeignKey(x => x.OrganizationId);

        // Enums
        builder.Property(x => x.Gender)
            .IsRequired()
            .HasConversion<int>(); // Update the conversion method

        builder.Property(x => x.Gender)
            .HasDefaultValue(GenderEnum.Male);
    }
}
