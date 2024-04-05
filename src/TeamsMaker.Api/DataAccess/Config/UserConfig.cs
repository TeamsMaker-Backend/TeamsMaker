using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TeamsMaker.Api.Core.Consts;
using TeamsMaker.Core.Enums;

namespace TeamsMaker.Api.DataAccess.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), DatabaseSchemas.Dbo);

        // Properties
        builder.Property(x => x.FirstName)
            .HasMaxLength(255);

        builder.Property(x => x.LastName)
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

        builder.Property(x => x.Email)
            .IsRequired();

        builder
            .OwnsOne(x => x.Avatar);

        builder
            .OwnsOne(x => x.Header);

        builder.HasIndex(x => x.Email).IsUnique();

        // Relationships
        builder
            .HasOne(x => x.Organization)
            .WithMany(y => y.Users)
            .HasForeignKey(x => x.OrganizationId);

        builder
            .HasMany(x => x.Links)
            .WithOne(y => y.User)
            .HasForeignKey(y => y.UserId)
            .IsRequired(false);

        builder
            .HasMany(x => x.MemberOn)
            .WithOne(y => y.User)
            .HasForeignKey(y => y.UserId);


        builder
            .HasMany(x => x.Upvotes)
            .WithOne(y => y.User)
            .HasForeignKey(y => y.UserId);

        // Enums
        builder.Property(x => x.Gender)
            .HasConversion<int>(); // Update the conversion method

        builder.Property(x => x.Gender)
            .HasDefaultValue(GenderEnum.Unknown);
    }
}
