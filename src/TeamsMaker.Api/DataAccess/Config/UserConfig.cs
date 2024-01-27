using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Properties
        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(x => x.SSN)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(x => x.Bio)
            .HasMaxLength(500);
        
        builder.Property(x => x.About)
            .HasMaxLength(2000);
        
        builder.Property(x => x.City)
            .HasMaxLength(255);

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
